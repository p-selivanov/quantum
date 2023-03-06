using System;
using System.Collections.Generic;

namespace Quantum.Lib.Kafka;

public class MessageConsumersConfig
{
    private readonly List<TopicConsumerConfig> _topicConsumerConfigs;

    internal IEnumerable<TopicConsumerConfig> TopicConsumerConfigs => _topicConsumerConfigs;

    /// <summary>
    /// Gets or sets initial list of brokers as a CSV list of broker host or host:port. 
    /// </summary>
    public string BootstrapServers { get; set; }

    /// <summary>
    /// Gets or sets Group ID. 
    /// </summary>
    public string GroupId { get; set; }

    public MessageConsumersConfig()
    {
        _topicConsumerConfigs = new List<TopicConsumerConfig>();
    }

    public void AddTopicConsumer(string topicName, Action<TopicConsumerConfig> configure)
    {
        if (configure is null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var topicConfig = new TopicConsumerConfig(topicName);
        configure.Invoke(topicConfig);

        topicConfig.Validate();

        _topicConsumerConfigs.Add(topicConfig);
    }

    public void Validate()
    {
        if (string.IsNullOrEmpty(BootstrapServers))
        {
            throw new Exception("BootstrapServers must not be empty.");
        }

        if (string.IsNullOrEmpty(GroupId))
        {
            throw new Exception("GroupId must not be empty.");
        }
    }
}
