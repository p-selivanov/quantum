using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quantum.Lib.Kafka.Extensions;
using Quantum.Lib.Kafka.Serialization;
using Quantum.Lib.Kafka.Telemetry;

namespace Quantum.Lib.Kafka;

public class StrictTopicConsumer : BackgroundService
{
    // Consumer retry count after which consumer health is considered Degraded.
    private const int RetryCountConsideredDegraded = 3;

    // Max consumer retry count. After it the consumer terminates and sets health to Unhealthy.
    private const int MaxRetryCount = 30;

    private readonly MessageConsumersConfig _kafkaConfig;
    private readonly TopicConsumerConfig _consumerConfig;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger _logger;
    private IConsumer<string, MessageValueEnvelope> _consumer;
    private int _clientId = 0;

    public StrictTopicConsumer(
        MessageConsumersConfig kafkaConfig,
        TopicConsumerConfig consumerConfig,
        IServiceScopeFactory scopeFactory,
        ILogger<StrictTopicConsumer> logger)
    {
        _kafkaConfig = kafkaConfig;
        _consumerConfig = consumerConfig;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    private Task InitializeAsync()
    {
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _kafkaConfig.BootstrapServers,
            GroupId = _kafkaConfig.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoOffsetStore = false,
            EnableAutoCommit = true,
            AutoCommitIntervalMs = 5000,
        };

        var consumerBuilder = new ConsumerBuilder<string, MessageValueEnvelope>(consumerConfig);
        consumerBuilder.SetErrorHandler();
        consumerBuilder.SetLogHandler(_logger);

        var deserializer = new TypePrefixJsonDeserializer(_consumerConfig.MessageTypes);
        consumerBuilder.SetValueDeserializer(deserializer);

        consumerBuilder.SetPartitionAssignmentLogHandler(_logger);

        _consumer = consumerBuilder.Build();
        _clientId = _consumer.GetClientId();

        Log("client created");

        return Task.CompletedTask;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(async () =>
        {
            try
            {
                Log("consumer starting");

                await InitializeAsync();
                KafkaClientHealthStore.ReportHealthy(_consumer.Name);

                _consumer.Subscribe(_consumerConfig.TopicName);
                Log("consumer subscribed");

                while (!stoppingToken.IsCancellationRequested)
                {
                    var result = _consumer.Consume(stoppingToken);

                    await ConsumeMessageAsync(result, stoppingToken);

                    KafkaClientHealthStore.ReportHealthy(_consumer.Name);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (ConsumeException consumeEx)
            {
                Log($"consumer terminated unexpectedly", consumeEx.ConsumerRecord.GetMessageTraceInfo(), level: LogLevel.Error, exception: consumeEx);
                KafkaClientHealthStore.ReportUnhealthy(_consumer?.Name ?? "null");
            }
            catch (Exception ex)
            {
                Log("consumer terminated unexpectedly", level: LogLevel.Error, exception: ex);
                KafkaClientHealthStore.ReportUnhealthy(_consumer?.Name ?? "null");
            }
            finally
            {
                _consumer?.Close();
                Log("consumer stopped");
            }
        });
    }

    private async Task ConsumeMessageAsync(ConsumeResult<string, MessageValueEnvelope> consumeResult, CancellationToken cancellationToken = default)
    {
        var traceInfo = consumeResult.GetMessageTraceInfo();
        var key = consumeResult.Message.Key;
        var envelope = consumeResult.Message.Value;
        var timestamp = consumeResult.Message.Timestamp.UtcDateTime;
        var offset = consumeResult.Offset.Value;

        if (envelope is null)
        {
            Log("error. Message is null", traceInfo, level: LogLevel.Error);
            return;
        }

        if (envelope.Message is null)
        {
            if (_consumerConfig.SkipUnknownMessages)
            {
                Log("skipped", traceInfo, envelope.TypeName, LogLevel.Debug);
                _consumer.StoreOffset(consumeResult);
                return;
            }

            throw new Exception($"Message type {envelope.TypeName} is not supported. Register a consumer for it or enable SkipUnknownMessages.");
        }

        var messageTypeName = envelope.TypeName;
        var messageValueType = envelope.Message.GetType();
        var messageType = typeof(Message<>).MakeGenericType(messageValueType);
        var consumerType = typeof(IMessageConsumer<>).MakeGenericType(messageValueType);

        Log("started", traceInfo, messageTypeName);

        var retryAttempt = 0;
        while (true)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var messageConsumer = scope.ServiceProvider.GetService(consumerType);
                if (messageConsumer is null)
                {
                    Log("message consumer is not registered", traceInfo, messageTypeName, LogLevel.Error);
                    return;
                }

                var message = Activator.CreateInstance(messageType, new object[] { key, envelope.Message, timestamp, offset });

                var consumeMethod = consumerType.GetMethod("ConsumeAsync");
                await (Task)consumeMethod.Invoke(messageConsumer, new object[] { message, cancellationToken });

                _consumer.StoreOffset(consumeResult);

                Log("completed", traceInfo, messageTypeName);
                return;
            }
            catch (Exception ex)
            {
                Log("error", traceInfo, messageTypeName, LogLevel.Error, ex);
            }

            retryAttempt++;
            if (retryAttempt > MaxRetryCount)
            {
                throw new Exception($"reached max retry count ({MaxRetryCount})");
            }

            Log($"retrying (attempt {retryAttempt})", traceInfo, messageTypeName);
            if (retryAttempt > RetryCountConsideredDegraded)
            {
                KafkaClientHealthStore.ReportDegraded(_consumer.Name);
            }
            
            await Task.Delay(_consumerConfig.RetryDelayMs, cancellationToken);            
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        Log("consumer stopping");

        await base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        base.Dispose();

        _consumer?.Dispose();
    }

    private void Log(
        string message,
        MessageTraceInfo traceInfo = null,
        string messageType = null,
        LogLevel level = LogLevel.Information,
        Exception exception = null)
    {
        var prefix = _clientId <= 0 ? "Kafka" : $"Kafka-{_clientId}";

        if (messageType is not null)
        {
            message = $"{messageType} {message}";
        }

        if (traceInfo is null)
        {
            _logger.Log(
                level,
                exception,
                $"{prefix} {message}");
        }
        else
        {
            _logger.Log(
                level,
                exception,
                $"{prefix} CONSUME {{topic}} {{partition}}:{{offset}}:{{key}} {message}",
                _consumerConfig.TopicName,
                traceInfo.Partition,
                traceInfo.Offest,
                traceInfo.Key);
        }
    }
}