namespace Quantum.Lib.Kafka.Serialization;

public class MessageEnvelope
{
    public string TypeName { get; }

    public object Message { get; }

    public MessageEnvelope(string typeName, object message)
    {
        TypeName = typeName;
        Message = message;
    }
}
