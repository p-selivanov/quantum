using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Options;
using Quantum.Account.Api.Configuration;
using Quantum.Account.Api.Models;
using Quantum.Lib.DynamoDb;

namespace Quantum.Account.Api.Repositories;

public class AccountRepository
{
    private readonly IAmazonDynamoDB _client;
    private readonly string _tableName;

    public AccountRepository(IAmazonDynamoDB client, IOptions<DynamoTableOptions> tableOptions)
    {
        _client = client;
        _tableName = tableOptions.Value.AccountTransactionTableName;
    }

    public async Task<List<AccountInfo>> GetAccountsAsync(string customerId)
    {
        var response = await _client.QueryAsync(new QueryRequest
        {
            TableName = _tableName,
            KeyConditionExpression = "CustomerId = :customerId AND (TransactionId BETWEEN :delimiterFrom AND :delimiterTo)",
            ExpressionAttributeValues =
            {
                [":customerId"] = AttributeValueFactory.FromString(customerId),
                [":delimiterFrom"] = AttributeValueFactory.FromString("##"),
                [":delimiterTo"] = AttributeValueFactory.FromString("#~"),
            },
        });

        var accounts = response.Items
            .Select(CreateAccountDetail)
            .ToList();

        return accounts;
    }

    public async Task<AccountInfo> GetAccountAsync(string customerId, string currency)
    {
        currency = currency.ToUpper();

        var response = await _client.GetItemAsync(new GetItemRequest
        {
            TableName = _tableName,
            Key =
            {
                ["CustomerId"] = AttributeValueFactory.FromString(customerId),
                ["TransactionId"] = AttributeValueFactory.FromString($"#{currency}"),
            },
        });

        if (response.IsItemSet == false)
        {
            return null;
        }

        var account = CreateAccountDetail(response.Item);

        return account;
    }

    public async Task EnsureAccountInitializedAsync(string customerId, string currency)
    {
        currency = currency.ToUpper();

        try
        {
            var response = await _client.PutItemAsync(new PutItemRequest
            {
                TableName = _tableName,
                Item =
                {
                    ["CustomerId"] = AttributeValueFactory.FromString(customerId),
                    ["TransactionId"] = AttributeValueFactory.FromString($"#{currency}"),
                    ["Balance"] = AttributeValueFactory.FromDecimal(0),
                },
                ConditionExpression = "attribute_not_exists(CustomerId)",
            });
        }
        catch (ConditionalCheckFailedException)
        {
        }
    }

    public AccountInfo CreateAccountDetail(Dictionary<string, AttributeValue> dbItem)
    {
        return new AccountInfo
        {
            CustomerId = dbItem["CustomerId"].AsString(),
            Currency = ParseTransactionCurrency(dbItem["TransactionId"].AsString()),
            Balance = dbItem["Balance"].AsDecimal(),
        };
    }

    private string ParseTransactionCurrency(string transactionId)
    {
        return transactionId.Substring(1);
    }
}
