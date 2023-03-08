using System;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Microsoft.Extensions.DependencyInjection;

namespace Quantum.Lib.DynamoDb;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDynamoDbClient(this IServiceCollection services, Action<DynamoDbOptions> configure) 
    {
        if (configure is null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var options = new DynamoDbOptions();
        configure.Invoke(options);

        services.AddSingleton<IAmazonDynamoDB>(sp =>
        {
            var config = new AmazonDynamoDBConfig();
            config.RetryMode = RequestRetryMode.Standard;
            config.MaxErrorRetry = 3;
            config.Timeout = TimeSpan.FromSeconds(10);

            if (string.Equals(options.Region, "localstack", StringComparison.OrdinalIgnoreCase))
            {
                config.RegionEndpoint = RegionEndpoint.USWest1;
                if (string.IsNullOrEmpty(options.LocalStackUri) is false)
                {
                    config.ServiceURL = options.LocalStackUri;
                }
                else
                {
                    config.ServiceURL = DynamoDbOptions.DefaultLocalStackUri;
                }
                return new AmazonDynamoDBClient("fakeAccessKeyId", "fakeAccessKey", config);
            }
            else
            {
                config.RegionEndpoint = RegionEndpoint.GetBySystemName(options.Region);
                return new AmazonDynamoDBClient(config);
            }
        });

        return services;
    }
}
