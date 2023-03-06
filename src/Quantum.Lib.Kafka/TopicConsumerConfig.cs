using System;
using System.Collections.Generic;
using System.Linq;

namespace Quantum.Lib.Kafka;

public class TopicConsumerConfig
{
    internal class MessageConsumerInfo
    {
        public Type Interface { get; }
        public Type Type { get; }

        public MessageConsumerInfo(Type @interface, Type type)
        {
            Interface = @interface;
            Type = type;
        }
    }

    private readonly List<MessageConsumerInfo> _messageConsumers;
    private readonly List<Type> _messageTypes;

    internal IEnumerable<MessageConsumerInfo> MessageConsumers => _messageConsumers;
    internal IEnumerable<Type> MessageTypes => _messageTypes;

    public string TopicName { get; }

    /// <summary>
    /// Get or sets the degree of parallelism. How many threads will consume one topic.
    /// Must be between 1 and 32 inclusive
    /// Default 1.
    /// </summary>
    public int DegreeOfParallelism { get; set; } = 1;

    /// <summary>
    /// Get or sets whether topic consumer will skip unknown message types.
    /// Default false.
    /// </summary>
    public bool SkipUnknownMessages { get; set; }

    /// <summary>
    /// Get or sets the retry delay milliseconds.
    /// Must be greater than 0.
    /// Default 500.
    /// </summary>
    public int RetryDelayMs { get; set; } = 500;

    public TopicConsumerConfig(string topicName)
    {
        TopicName = topicName;
        _messageConsumers = new List<MessageConsumerInfo>();
        _messageTypes = new List<Type>();
    }

    public void AddMessageConsumer<TConsumer>()
        where TConsumer : class
    {
        var candidateType = typeof(TConsumer);
        var interfaceType = typeof(IMessageConsumer<>);
        
        var candidateInterfaces = candidateType
            .GetInterfaces()
            .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == interfaceType)
            .ToList();

        if (!candidateInterfaces.Any())
        {
            throw new ArgumentException($"Type '{candidateType.FullName}' does not implement 'IMessageConsumer<>'.");
        }

        foreach (var candidateInterface in candidateInterfaces)
        {
            _messageConsumers.Add(new MessageConsumerInfo(candidateInterface, candidateType));

            var messageType = candidateInterface.GetGenericArguments().First();
            _messageTypes.Add(messageType);
        }
    }

    public void Validate()
    {
        if (DegreeOfParallelism < 1 ||
            DegreeOfParallelism > 32)
        {
            throw new Exception("DegreeOfParallelism must be between 1 and 32 inclusive.");
        }

        if (RetryDelayMs < 0)
        {
            throw new Exception("RetryDelayMs must be greater then 0.");
        }
    }
}
