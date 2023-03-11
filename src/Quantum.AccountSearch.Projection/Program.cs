using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quantum.AccountSearch.Persistence;
using Quantum.AccountSearch.Projection.Repositories;
using Quantum.Lib.Kafka;

namespace Quantum.AccountSearch.Projection;

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
        services.AddDbContext<AccountSearchDbContext>(options =>
            options.UseNpgsql(context.Configuration.GetValue<string>("AccountSearchDb:ConnectionString")));

        services.AddKafkaMessageConsumers(kafkaConfig =>
        {
            kafkaConfig.BootstrapServers = context.Configuration.GetValue<string>("Kafka:BootstrapServers");
            kafkaConfig.GroupId = "AccountSearch.Projection";

            var topic = context.Configuration.GetValue<string>("Kafka:Topics:CustomerEvents");

            kafkaConfig.AddTopicConsumer(topic, topicConfig =>
            {
                topicConfig.DegreeOfParallelism = context.Configuration.GetValue<int>("Kafka:DegreeOfParallelism");
                topicConfig.SkipUnknownMessages = true;
                topicConfig.AddMessageConsumer<CustomerEventConsumer>();
            });
        });

        services.AddScoped<CustomerAccountRepository>();
    }
}