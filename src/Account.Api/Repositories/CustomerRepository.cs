using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Options;
using Quantum.Acccount.Api.Models;
using Quantum.Account.Api.Configuration;
using Quantum.Account.Api.Models;
using Quantum.Lib.DynamoDb;

namespace Quantum.Account.Api.Repositories;

public class CustomerRepository
{
    private readonly IAmazonDynamoDB _client;
    private readonly string _tableName;

    public CustomerRepository(IAmazonDynamoDB client, IOptions<DynamoTableOptions> tableOptions)
    {
        _client = client;
        _tableName = tableOptions.Value.AccountTransactionTableName;
    }

    public async Task<CustomerInfo> GetCustomerAsync(string customerId)
    {
        var response = await _client.GetItemAsync(new GetItemRequest
        {
            TableName = _tableName,
            Key =
            {
                ["CustomerId"] = AttributeValueFactory.FromString(customerId),
                ["TransactionId"] = AttributeValueFactory.FromString("#"),
            },
        });

        if (response.IsItemSet == false)
        {
            return null;
        }

        var customer = new CustomerInfo
        {
            Id = response.Item["CustomerId"].AsString(),
            Country = response.Item["Country"].AsString(),
            Status = response.Item["Status"].AsEnum<CustomerStatus>(),
        };

        return customer;
    }
}
