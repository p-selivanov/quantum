using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quantum.Account.CustomerConsumer.Configuration;
using Quantum.Account.CustomerConsumer.Repositories;
using Quantum.Lib.DynamoDb;
using Quantum.Lib.Kafka;

namespace Quantum.Account.CustomerConsumer;

public class Program
{
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(ConfigureServices)
            .Build();

        host.Run();
    }

    private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services.Configure<CustomerTableOptions>(options =>
        {
            options.CustomerTableName = context.Configuration.GetValue<string>("DynamoDB:Tables:AccountTransactions");
        });

        services.AddDynamoDbClient(options =>
        {
            options.Region = context.Configuration.GetValue<string>("DynamoDB:Region");
            options.LocalStackUri = context.Configuration.GetValue<string>("DynamoDB:LocalStackUri");
        });

        services.AddScoped<CustomerRepository>();

        services.AddKafkaMessageConsumers(kafkaConfig =>
        {
            kafkaConfig.BootstrapServers = context.Configuration.GetValue<string>("Kafka:BootstrapServers");
            kafkaConfig.GroupId = "Account.CustomerConsumer";

            var topic = context.Configuration.GetValue<string>("Kafka:Topics:CustomerEvents");

            kafkaConfig.AddTopicConsumer(topic, topicConfig =>
            {
                topicConfig.DegreeOfParallelism = context.Configuration.GetValue<int>("Kafka:DegreeOfParallelism");
                topicConfig.SkipUnknownMessages = true;
                topicConfig.AddMessageConsumer<CustomerEventConsumer>();
            });
        });
    }
}