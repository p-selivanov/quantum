using System.Text;

namespace Quantum.Lib.Kafka;

/// <summary>
/// Represents message trace information.
/// </summary>
public class MessageTraceInfo
{
    public int Partition { get; }

    public long Offest { get; }

    public string Key { get; }

    public MessageTraceInfo(int partition, long offset, object key)
    {
        Partition = partition;
        Offest = offset;

        if (key is null)
        {
            Key = "null";
        }
        else if (key is byte[] keyBytes)
        {
            Key = Encoding.UTF8.GetString(keyBytes);
        }
        else
        {
            Key = key.ToString();
        }
    }

    public override string ToString()
    {
        return $"{Partition}:{Offest}:{Key}";
    }
}