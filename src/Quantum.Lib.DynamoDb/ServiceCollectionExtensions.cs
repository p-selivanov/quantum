using System;
using Amazon;
using Amazon.DynamoDBv2;
using Microsoft.Extensions.DependencyInjection;

namespace Quantum.Lib.DynamoDb;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDynamoDbClient(this IServiceCollection services, string region) 
    {
        services.AddSingleton<IAmazonDynamoDB>(sp =>
        {
            var config = new AmazonDynamoDBConfig();
            if (string.Equals(region, "localstack", StringComparison.OrdinalIgnoreCase))
            {
                config.RegionEndpoint = RegionEndpoint.USWest1;
                config.ServiceURL = "http://localhost:4566";
                return new AmazonDynamoDBClient("fakeAccessKeyId", "fakeAccessKey", config);
            }
            else
            {
                config.RegionEndpoint = RegionEndpoint.GetBySystemName(region);
                return new AmazonDynamoDBClient(config);
            }
        });

        return services;
    }
}
