using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quantum.Lib.Kafka.Extensions;
using Quantum.Lib.Kafka.Serialization;
using Quantum.Lib.Kafka.Telemetry;

namespace Quantum.Lib.Kafka;

public class StrictTopicConsumer : TopicConsumerBase<string, MessageEnvelope>
{
    // Consumer retry count after which consumer health is considered Degraded.
    private const int RetryCountConsideredDegraded = 3;

    // Max consumer retry count. After it the consumer terminates and sets health to Unhealthy.
    private const int MaxRetryCount = 30;

    private readonly MessageConsumersConfig kafkaConfig;
    private readonly TopicConsumerConfig consumerConfig;
    private readonly IServiceScopeFactory scopeFactory;

    public StrictTopicConsumer(
        MessageConsumersConfig kafkaConfig,
        TopicConsumerConfig consumerConfig,
        IServiceScopeFactory scopeFactory,
        ILogger<StrictTopicConsumer> logger)
        : base(kafkaConfig.BootstrapServers, consumerConfig.TopicName, logger)
    {
        this.kafkaConfig = kafkaConfig;
        this.consumerConfig = consumerConfig;
        this.scopeFactory = scopeFactory;
    }

    protected override void ConfigureInternalConsumer(ConsumerConfig consumerConfig)
    {
        consumerConfig.GroupId = this.kafkaConfig.GroupId;
        consumerConfig.AutoOffsetReset = AutoOffsetReset.Earliest;
        consumerConfig.EnableAutoOffsetStore = false;
        consumerConfig.EnableAutoCommit = true;
        consumerConfig.AutoCommitIntervalMs = 5000;
    }

    protected override void ConfigureInternalConsumerBuilder(ConsumerBuilder<string, MessageEnvelope> consumerBuilder)
    {
        base.ConfigureInternalConsumerBuilder(consumerBuilder);

        var deserializer = new TypePrefixJsonDeserializer(this.consumerConfig.MessageTypes);
        consumerBuilder.SetValueDeserializer(deserializer);

        consumerBuilder.SetPartitionAssignmentLogHandler(_logger);
    }

    protected override async Task ConsumeMessageAsync(ConsumeResult<string, MessageEnvelope> consumeResult, CancellationToken cancellationToken = default)
    {
        var traceInfo = consumeResult.GetMessageTraceInfo();
        var key = consumeResult.Message.Key;
        var envelope = consumeResult.Message.Value;

        if (envelope is null)
        {
            Log("error. Message is null", traceInfo, null, LogLevel.Error);
            return;
        }

        if (envelope.Message is null)
        {
            if (this.consumerConfig.SkipUnknownMessages)
            {
                Log("skipped", traceInfo, envelope.TypeName, LogLevel.Debug);
                _consumer.StoreOffset(consumeResult);
                return;
            }

            throw new Exception($"Message type {envelope.TypeName} is not supported. Register a consumer for it or enable SkipUnknownMessages.");
        }

        var messageTypeName = envelope.TypeName;
        var messageType = envelope.Message.GetType();
        var consumerType = typeof(IMessageConsumer<>).MakeGenericType(messageType);

        Log("started", traceInfo, messageTypeName);

        var retryAttempt = 0;
        while (true)
        {
            try
            {
                using var scope = this.scopeFactory.CreateScope();

                var messageConsumer = scope.ServiceProvider.GetService(consumerType);
                if (messageConsumer is null)
                {
                    Log("error. Message consumer is not registered", traceInfo, messageTypeName);
                    return;
                }

                var consumeMethod = consumerType.GetMethod("ConsumeAsync");
                await (Task)consumeMethod.Invoke(messageConsumer, new object[] { key, envelope.Message, cancellationToken });

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
            
            await Task.Delay(this.consumerConfig.RetryDelayMs, cancellationToken);            
        }
    }

    private void Log(string message, MessageTraceInfo traceInfo, string messageType, LogLevel level = LogLevel.Information, Exception exception = null)
    {
        base.Log($"{messageType} {message}", traceInfo, level, exception);
    }
}