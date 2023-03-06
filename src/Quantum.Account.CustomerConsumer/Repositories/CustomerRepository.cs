using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Options;
using Quantum.Account.CustomerConsumer.Configuration;
using Quantum.Lib.Common;
using Quantum.Lib.DynamoDb;

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

    public async Task CreateCustomerAsync(string customerId, string country, string status)
    {
        await _client.PutItemAsync(new PutItemRequest
        {
            TableName = _tableName,
            Item =
            {
                ["CustomerId"] = AttributeValueFactory.FromString(customerId),
                ["TransactionId"] = AttributeValueFactory.FromString("#"),
                ["Status"] = AttributeValueFactory.FromString(status),
                ["Country"] = AttributeValueFactory.FromString(country),
            },
        });
    }

    public async Task<bool> UpdateCustomerAsync(string customerId, Specifiable<string> country, Specifiable<string> status)
    {
        if (country.IsSpecified is false &&
            status.IsSpecified is false)
        {
            return false;
        }

        var request = new UpdateItemRequest
        {
            TableName = _tableName,
            Key = {
                ["CustomerId"] = AttributeValueFactory.FromString(customerId),
                ["TransactionId"] = AttributeValueFactory.FromString("#"),
            },
            UpdateExpression = "SET ",
            ConditionExpression = "CustomerId = :id",
            ExpressionAttributeValues =
            {
                [":id"] = AttributeValueFactory.FromString(customerId),
            },
        };

        if (country.IsSpecified)
        {
            request.UpdateExpression += "Country = :country";
            request.ExpressionAttributeValues[":country"] = AttributeValueFactory.FromString(country.Value);
        }

        if (status.IsSpecified)
        {
            if (country.IsSpecified)
            {
                request.UpdateExpression += ", ";
            }

            request.UpdateExpression += "Status = :status";
            request.ExpressionAttributeValues[":status"] = AttributeValueFactory.FromString(status.Value);
        }

        try
        {
            await _client.UpdateItemAsync(request);
            return true;
        }
        catch (ConditionalCheckFailedException)
        {
            return false;
        }
    }
}
