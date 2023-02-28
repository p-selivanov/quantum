using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Quantum.Lib.DynamoDb;

namespace Quantum.Account.Api.Repositories;

public class TransactionRepository
{
    private const string TableName = "AccountTransactions";

    private readonly IAmazonDynamoDB _client;

    public TransactionRepository(IAmazonDynamoDB client)
    {
        _client = client;
    }

    public async Task<bool> CreateTransactionAsync(string accountId, decimal amount)
    {
        try
        {
            var timestamp = DateTime.UtcNow;

            await _client.TransactWriteItemsAsync(new TransactWriteItemsRequest
            {
                TransactItems =
                {
                    new TransactWriteItem
                    {
                        Update = new Update
                        {
                            TableName = TableName,
                            Key =
                            {
                                ["AccountId"] = AttributeValueFactory.FromString(accountId),
                                ["TransactionId"] = AttributeValueFactory.FromInt(0),
                            },
                            UpdateExpression = "ADD Balance :amount",
                            ConditionExpression = "AccountId = :accountId AND (Balance >= :minBalance)",
                            ExpressionAttributeValues =
                            {
                                [":accountId"] = AttributeValueFactory.FromString(accountId),
                                [":amount"] = AttributeValueFactory.FromDecimal(amount),
                                [":minBalance"] = AttributeValueFactory.FromDecimal(amount < 0m ? -amount : -1m),
                            },
                        }
                    },
                    new TransactWriteItem
                    {
                        Put = new Put
                        {
                            TableName = TableName,
                            Item =
                            {
                                ["AccountId"] = AttributeValueFactory.FromString(accountId),
                                ["TransactionId"] = AttributeValueFactory.FromTimestamp(timestamp),
                                ["Amount"] = AttributeValueFactory.FromDecimal(amount),
                            },
                        }
                    },
                }
            });

            return true;
        }
        catch (TransactionCanceledException)
        {
            return false;
        }
        catch (ConditionalCheckFailedException)
        {
            return false;
        }
    }
}
