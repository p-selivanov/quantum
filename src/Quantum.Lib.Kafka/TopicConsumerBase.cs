using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quantum.Lib.Kafka.Extensions;
using Quantum.Lib.Kafka.Telemetry;

namespace Quantum.Lib.Kafka;

/// <summary>
/// Represents an abstract topic consumer. Implemented as a hosted service. Contains only basic consumer routines.
/// </summary>
public abstract class TopicConsumerBase<TKey, TValue> : BackgroundService
{
    protected readonly string _bootstrapServers;
    protected readonly string _topicName;
    protected readonly ILogger _logger;
    protected IConsumer<TKey, TValue> _consumer;
    protected int _clientId = 0;

    public TopicConsumerBase(string bootstrapServers, string topicName, ILogger logger)
    {
        _bootstrapServers = bootstrapServers;
        _topicName = topicName;
        _logger = logger;
    }

    /// <summary>
    /// Runs the hosted service.
    /// </summary>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(async () =>
        {
            try
            {
                Log("consumer starting");

                await InitializeAsync();
                KafkaClientHealthStore.ReportHealthy(_consumer.Name);

                _consumer.Subscribe(_topicName);
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
                Log($"consumer terminated unexpectedly", consumeEx.ConsumerRecord.GetMessageTraceInfo(), LogLevel.Error, consumeEx);
                KafkaClientHealthStore.ReportUnhealthy(_consumer?.Name ?? "null");
            }
            catch (Exception ex)
            {
                Log("consumer terminated unexpectedly", null, LogLevel.Error, ex);
                KafkaClientHealthStore.ReportUnhealthy(_consumer?.Name ?? "null");
            }
            finally
            {
                _consumer?.Close();
                Log("consumer stopped");
            }
        });
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

    /// <summary>
    /// Initializes the consumer. Creates the internal Kafka consumer.
    /// </summary>
    protected virtual Task InitializeAsync()
    {
        var consumerConfig = new ConsumerConfig();
        consumerConfig.BootstrapServers = _bootstrapServers;
        consumerConfig.AllowAutoCreateTopics = true;

        ConfigureInternalConsumer(consumerConfig);

        var consumerBuilder = new ConsumerBuilder<TKey, TValue>(consumerConfig);
        consumerBuilder.SetErrorHandler();
        consumerBuilder.SetLogHandler(_logger);

        ConfigureInternalConsumerBuilder(consumerBuilder);

        _consumer = consumerBuilder.Build();
        _clientId = _consumer.GetClientId();

        Log("client created");

        return Task.CompletedTask;
    }

    /// <summary>
    /// Configures the internal Kafka consumer with custom settings.
    /// Must set GroupId.
    /// </summary>
    protected abstract void ConfigureInternalConsumer(ConsumerConfig consumerConfig);

    /// <summary>
    /// Configures the internal Kafka consumer builder with custom extensions.
    /// </summary>
    protected virtual void ConfigureInternalConsumerBuilder(ConsumerBuilder<TKey, TValue> consumerBuilder)
    {
    }

    /// <summary>
    /// Consumes a message received from the internal consumer.
    /// </summary>
    protected abstract Task ConsumeMessageAsync(ConsumeResult<TKey, TValue> consumeResult, CancellationToken cancellationToken = default);

    /// <summary>
    /// Writes log.
    /// If traceInfo is provided - logs Kafka message position (partition:offset:key).
    /// </summary>
    protected void Log(
        string message,
        MessageTraceInfo traceInfo = null,
        LogLevel level = LogLevel.Information,
        Exception exception = null)
    {
        var prefix = _clientId <= 0 ? "Kafka" : $"Kafka-{_clientId}";

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
                _topicName,
                traceInfo.Partition,
                traceInfo.Offest,
                traceInfo.Key);
        }
    }
}