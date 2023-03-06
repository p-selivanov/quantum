namespace Quantum.Lib.Kafka;

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
