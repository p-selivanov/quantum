using Confluent.Kafka;

namespace Quantum.Lib.Kafka.Extensions;

public static class ConsumeResultExtensions
{
    /// <summary>
    /// Creates MessageTraceInfo from a consume result. 
    /// </summary>
    public static MessageTraceInfo GetMessageTraceInfo<TKey, TValue>(this ConsumeResult<TKey, TValue> consumeResult)
    {
        return new MessageTraceInfo(consumeResult.Partition.Value, consumeResult.Offset.Value, consumeResult.Message.Key);
    }
}
