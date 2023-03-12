namespace Quantum.Lib.Kafka.Serialization;

public class MessageValueEnvelope
{
    public string TypeName { get; }

    public object Message { get; }

    public MessageValueEnvelope(string typeName, object message)
    {
        TypeName = typeName;
        Message = message;
    }
}
