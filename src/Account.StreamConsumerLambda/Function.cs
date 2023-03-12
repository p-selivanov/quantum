using Amazon.Lambda.Core;
using Amazon.Lambda.DynamoDBEvents;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Quantum.Lib.Kafka.Serialization;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Quantum.Account.StreamConsumerLambda;

public class Function
{
    private readonly IConfiguration _configuration;
    private IProducer<string, object> _producer;

    public Function()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();
    }

    public void FunctionHandler(DynamoDBEvent dynamoEvent, ILambdaContext context)
    {
        context.Logger.LogInformation($"Processing {dynamoEvent.Records.Count} records");

        EnsureProducerInitialized();
        var topicName = _configuration.GetValue<string>("Kafka:Topics:CustomerEvents");

        foreach (var record in dynamoEvent.Records)
        {
            context.Logger.LogInformation($"{record.EventID}: processing");

            var message = MessageFactory.ExtractMessage(record);
            if (message is null)
            {
                context.Logger.LogInformation($"{record.EventID}: skipped");
                continue;
            }

            _producer.Produce(topicName, message);

            context.Logger.LogInformation($"{record.EventID}: produced {message.Value.GetType().Name}");
        }

        context.Logger.LogInformation($"Processing {dynamoEvent.Records.Count} records completed");
    }

    private void EnsureProducerInitialized()
    {
        if (_producer is not null)
        {
            return;
        }

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = _configuration.GetValue<string>("Kafka:BootstrapServers"),
            EnableIdempotence = true,
            Partitioner = Partitioner.Consistent,
            MessageSendMaxRetries = 3,
            ReconnectBackoffMs = 1000,
            RetryBackoffMs = 1000,
            Acks = Acks.All,
            EnableDeliveryReports = true,
            DeliveryReportFields = "none",
        };

        var producerBuilder = new ProducerBuilder<string, object>(producerConfig);
        producerBuilder.SetValueSerializer(new TypePrefixJsonSerializer());
        
        _producer = producerBuilder.Build();
    }
}