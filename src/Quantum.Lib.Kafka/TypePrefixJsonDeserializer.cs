using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Confluent.Kafka;
using Quantum.Lib.Common;

namespace Quantum.Lib.Kafka;

public class TypePrefixJsonDeserializer : IDeserializer<MessageEnvelope>
{
    private readonly Dictionary<string, Type> messageTypes;

    public TypePrefixJsonDeserializer(IEnumerable<Type> messageTypes)
    {
        if (messageTypes is null)
        {
            throw new ArgumentNullException(nameof(messageTypes));
        }

        this.messageTypes = messageTypes
            .ToDictionary(x => x.Name, x => x);
    }

    public TypePrefixJsonDeserializer(Dictionary<string, Type> messageTypeMap)
    {
        if (messageTypeMap is null)
        {
            throw new ArgumentNullException(nameof(messageTypeMap));
        }

        this.messageTypes = messageTypeMap;
    }

    public MessageEnvelope Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull)
        {
            return null;
        }

        var messageString = Encoding.UTF8.GetString(data.ToArray());
        var separatorIndex = messageString.IndexOf('\n');
        if (separatorIndex < 0)
        {
            throw new Exception("Message format is invalid");
        }

        var messageTypeName = messageString.Substring(0, separatorIndex);
        if (string.IsNullOrEmpty(messageTypeName))
        {
            throw new Exception("Message format is invalid");
        }

        var messageContentString = messageString.Substring(separatorIndex + 1);

        if (!messageTypes.TryGetValue(messageTypeName, out var messageType))
        {
            return new MessageEnvelope(messageTypeName, null);
        }

        using var reader = new StringReader(messageContentString);
        var message = QuantumJson.DefaultSerializer.Deserialize(reader, messageType);

        return new MessageEnvelope(messageTypeName, message);
    }
}