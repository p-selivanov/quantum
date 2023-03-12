using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Quantum.Lib.Kafka;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaMessageConsumers(this IServiceCollection services, Action<MessageConsumersConfig> configure)
    {
        if (configure is null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        if (services.Any(x => x.ServiceType == typeof(MessageConsumersConfig)))
        {
            throw new Exception("Kafka Message Consumers cannot be added more than once.");
        }

        var config = new MessageConsumersConfig();
        configure.Invoke(config);

        config.Validate();

        services.AddSingleton(config);

        foreach (var topicConsumerConfig in config.TopicConsumerConfigs)
        {
            for (var i = 0; i < topicConsumerConfig.DegreeOfParallelism; i++)
            {
                services.AddSingleton<IHostedService>(sp =>
                    new StrictTopicConsumer(
                        config,
                        topicConsumerConfig,
                        sp.GetRequiredService<IServiceScopeFactory>(),
                        sp.GetRequiredService<ILogger<StrictTopicConsumer>>())
                    );
            }

            foreach (var messageConsumer in topicConsumerConfig.MessageConsumers)
            {
                services.TryAddTransient(messageConsumer.Interface, messageConsumer.Type);
            }
        }

        return services;
    }
}