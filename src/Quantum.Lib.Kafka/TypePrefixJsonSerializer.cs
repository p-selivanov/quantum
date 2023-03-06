using System.IO;
using Confluent.Kafka;
using Quantum.Lib.Common;

namespace Quantum.Lib.Kafka;

public class TypePrefixJsonSerializer : ISerializer<object>
{
    public byte[] Serialize(object data, SerializationContext context)
    {
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream);

        writer.Write(data.GetType().Name);
        writer.Write('\n');

        QuantumJson.DefaultSerializer.Serialize(writer, data);
        writer.Flush();

        return stream.ToArray();
    }
}