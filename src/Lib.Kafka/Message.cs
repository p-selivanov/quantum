using System;

namespace Quantum.Lib.Kafka;

public class Message<TValue>
{
    public string Key { get; set; }

    public TValue Value { get; set; }

    public DateTime Timestamp { get; set; }

    public long Offset { get; set; }

    public Message(string key, TValue value, DateTime timestamp, long offset)
    {
        Key = key;
        Value = value;
        Timestamp = timestamp;
        Offset = offset;
    }
}
