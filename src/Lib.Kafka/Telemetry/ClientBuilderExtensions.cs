using System.Linq;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Quantum.Lib.Kafka.Extensions;

namespace Quantum.Lib.Kafka.Telemetry;

public static class ClientBuilderExtensions
{
    /// <summary>
    /// Sets a log handler that in case of an error writes log.
    /// </summary>
    public static void SetLogHandler<K, V>(this ProducerBuilder<K, V> builder, ILogger logger, string logPrefix = "Kafka")
    {
        builder.SetLogHandler((producer, message) =>
        {
            if (message.Level == SyslogLevel.Error)
            {
                logger.LogError($"{logPrefix}-{producer.GetClientId()} producer error: " + message.Message);
            }
            else if (message.Level == SyslogLevel.Critical ||
                message.Level == SyslogLevel.Emergency)
            {
                logger.LogCritical($"{logPrefix}-{producer.GetClientId()} producer critical error: " + message.Message);
            }
            else
            {
                logger.LogDebug($"{logPrefix}-{producer.GetClientId()}: " + message.Message);
            }
        });
    }

    /// <summary>
    /// Sets a log handler that in case of an error writes log.
    /// </summary>
    public static void SetLogHandler<K, V>(this ConsumerBuilder<K, V> builder, ILogger logger, string logPrefix = "Kafka")
    {
        builder.SetLogHandler((consumer, message) =>
        {
            if (message.Level == SyslogLevel.Error)
            {
                logger.LogError($"{logPrefix}-{consumer.GetClientId()} consumer error: " + message.Message);
            }
            else if (message.Level == SyslogLevel.Critical ||
                message.Level == SyslogLevel.Emergency)
            {
                logger.LogCritical($"{logPrefix}-{consumer.GetClientId()} consumer critical error: " + message.Message);
            }
            else
            {
                logger.LogDebug($"{logPrefix}-{consumer.GetClientId()}: " + message.Message);
            }
        });
    }

    /// <summary>
    /// Sets 3 partition assignment handlers that logs what partitions are assigned, revoked or lost.
    /// </summary>
    public static void SetPartitionAssignmentLogHandler<K, V>(this ConsumerBuilder<K, V> builder, ILogger logger, string logPrefix = "Kafka")
    {
        builder.SetPartitionsAssignedHandler((consumer, partitions) =>
        {
            var assignedPartitionIds = partitions
                .Select(x => x.Partition.Value);

            logger.LogInformation(
                $"{logPrefix}-{consumer.GetClientId()} partitions assigned: {string.Join(", ", assignedPartitionIds)}");
        });

        builder.SetPartitionsRevokedHandler((consumer, partitions) =>
        {
            var revokedPartitionIds = partitions
                .Select(x => x.Partition.Value);

            var remainingPartitionIds = consumer.Assignment
                .Where(x => !partitions.Any(y => y.TopicPartition == x))
                .Select(x => x.Partition.Value);

            logger.LogInformation(
                $"{logPrefix}-{consumer.GetClientId()} partitions revoked: {string.Join(", ", revokedPartitionIds)}. " +
                $"Remaining: {string.Join(", ", remainingPartitionIds)}");
        });

        builder.SetPartitionsLostHandler((consumer, partitions) =>
        {
            var loastPartitionIds = partitions
                .Select(x => x.Partition.Value);

            var remainingPartitionIds = consumer.Assignment
                .Where(x => !partitions.Any(y => y.TopicPartition == x))
                .Select(x => x.Partition.Value);

            logger.LogInformation(
                $"{logPrefix}-{consumer.GetClientId()} partitions lost: {string.Join(", ", loastPartitionIds)}. " +
                $"Remaining: {string.Join(", ", remainingPartitionIds)}");
        });
    }

    /// <summary>
    /// Sets an error handler that in case of an error updates health check.
    /// </summary>
    public static void SetErrorHandler<K, V>(this ProducerBuilder<K, V> builder)
    {
        builder.SetErrorHandler((producer, error) =>
        {
            switch (error.Code)
            {
                // we consider single Local_Transport errors as not fatal
                // because in case all brokers are down we receive Local_AllBrokersDown error
                case ErrorCode.Local_Transport:
                    KafkaClientHealthStore.ReportDegraded(producer.Name);
                    break;
                default:
                    KafkaClientHealthStore.ReportUnhealthy(producer.Name);
                    break;
            }
        });
    }
    
    /// <summary>
    /// Sets an error handler that in case of an error updates health check.
    /// </summary>
    public static void SetErrorHandler<K, V>(this ConsumerBuilder<K, V> builder)
    {
        builder.SetErrorHandler((consumer, error) =>
        {
            switch (error.Code)
            {
                // we consider single Local_Transport errors as not fatal
                // because in case all brokers are down we receive Local_AllBrokersDown error
                case ErrorCode.Local_Transport:
                    KafkaClientHealthStore.ReportDegraded(consumer.Name);
                    break;
                default:
                    KafkaClientHealthStore.ReportUnhealthy(consumer.Name);
                    break;
            }
        });
    }
}
