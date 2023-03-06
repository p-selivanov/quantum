using Amazon.DynamoDBv2;
using Microsoft.Extensions.Options;
using Quantum.Account.CustomerConsumer.Configuration;

namespace Quantum.Account.CustomerConsumer.Repositories;

internal class CustomerRepository
{
    private readonly IAmazonDynamoDB _client;
    private readonly string _tableName;

    public CustomerRepository(IAmazonDynamoDB client, IOptions<CustomerTableOptions> tableOptions)
    {
        _client = client;
        _tableName = tableOptions.Value.CustomerTableName;
    }
}
