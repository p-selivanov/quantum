using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Quantum.Account.Api.Models;
using Quantum.Lib.DynamoDb;

namespace Quantum.Account.Api.Repositories;

public class AccountRepository
{
    private const string TableName = "AccountTransactions";

    private readonly IAmazonDynamoDB _client;

    public AccountRepository(IAmazonDynamoDB client)
    {
        _client = client;
    }

    public async Task<AccountDetail> GetAccount(string accountId)
    {
        var response = await _client.GetItemAsync(new GetItemRequest
        {
            TableName = TableName,
            Key =
            {
                ["AccountId"] = AttributeValueFactory.FromString(accountId),
                ["TransactionId"] = AttributeValueFactory.FromInt(0),
            },
        });

        if (response.IsItemSet == false)
        {
            return null;
        }

        var account = new AccountDetail
        {
            Id = response.Item["AccountId"].AsString(),
            CustomerId = response.Item["CustomerId"].AsString(),
            Currency = response.Item["Currency"].AsString(),
            Balance = response.Item["Balance"].AsDecimal(),
        };

        return account;
    }

    public async Task CreateAccount(string accountId, string customerId, string currency)
    {
        var response = await _client.PutItemAsync(new PutItemRequest
        {
            TableName = TableName,
            Item =
            {
                ["AccountId"] = AttributeValueFactory.FromString(accountId),
                ["TransactionId"] = AttributeValueFactory.FromInt(0),
                ["CustomerId"] = AttributeValueFactory.FromString(customerId),
                ["Currency"] = AttributeValueFactory.FromString(currency),
                ["Balance"] = AttributeValueFactory.FromDecimal(0),
            },
        });
    }
}
