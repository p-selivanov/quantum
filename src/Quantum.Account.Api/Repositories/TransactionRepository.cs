using System;
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

public class TransactionRepository
{
    private readonly IAmazonDynamoDB _client;
    private readonly string _tableName;

    public TransactionRepository(IAmazonDynamoDB client, IOptions<DynamoTableOptions> tableOptions)
    {
        _client = client;
        _tableName = tableOptions.Value.AccountTransactionTableName;
    }

    public async Task<List<TransactionInfo>> GetTransactionsAsync(string customerId, string currency)
    {
        currency = currency.ToUpper();

        var response = await _client.QueryAsync(new QueryRequest
        {
            TableName = _tableName,
            KeyConditionExpression = "CustomerId = :customerId AND (TransactionId BETWEEN :delimiterFrom AND :delimiterTo)",
            ExpressionAttributeValues =
            {
                [":customerId"] = AttributeValueFactory.FromString(customerId),
                [":delimiterFrom"] = AttributeValueFactory.FromString(currency),
                [":delimiterTo"] = AttributeValueFactory.FromString($"{currency}~"),
            },
        });

        var transactions = response.Items
            .Select(x => new TransactionInfo
            {
                Currency = ParseTransactionCurrency(x["TransactionId"].AsString()),
                Amount = x["Amount"].AsDecimal(),
                Commission = x["Commission"].AsDecimal(),
                Timestamp = ParseTransactionTimestamp(x["TransactionId"].AsString()),
            })
            .ToList();

        return transactions;
    }

    public async Task<string> CreateDepositTransactionAsync(string customerId, string currency, decimal amount, decimal commission)
    {
        currency = currency.ToUpper();
        var transactionId = CreateTransctionId(currency);

        try
        {
            await _client.TransactWriteItemsAsync(new TransactWriteItemsRequest
            {
                TransactItems =
                {
                    new TransactWriteItem
                    {
                        Update = new Update
                        {
                            TableName = _tableName,
                            Key =
                            {
                                ["CustomerId"] = AttributeValueFactory.FromString(customerId),
                                ["TransactionId"] = AttributeValueFactory.FromString($"#{currency}"),
                            },
                            UpdateExpression = "ADD Balance :amount",
                            ExpressionAttributeValues =
                            {
                                [":amount"] = AttributeValueFactory.FromDecimal(amount),
                            },
                        }
                    },
                    new TransactWriteItem
                    {
                        Put = CreateTransactionPut(customerId, transactionId, amount, commission),
                    },
                }
            });

            return transactionId;
        }
        catch (TransactionCanceledException)
        {
            return null;
        }
    }

    public async Task<string> CreateWithdrawalTransactionAsync(string customerId, string currency, decimal amount, decimal commission)
    {
        currency = currency.ToUpper();
        var transactionId = CreateTransctionId(currency);

        try
        {
            await _client.TransactWriteItemsAsync(new TransactWriteItemsRequest
            {
                TransactItems =
                {
                    new TransactWriteItem
                    {
                        Update = new Update
                        {
                            TableName = _tableName,
                            Key =
                            {
                                ["CustomerId"] = AttributeValueFactory.FromString(customerId),
                                ["TransactionId"] = AttributeValueFactory.FromString($"#{currency}"),
                            },
                            UpdateExpression = "ADD Balance :amount",
                            ConditionExpression = "CustomerId = :customerId AND Balance >= :minBalance",
                            ExpressionAttributeValues =
                            {
                                [":customerId"] = AttributeValueFactory.FromString(customerId),
                                [":amount"] = AttributeValueFactory.FromDecimal(-amount),
                                [":minBalance"] = AttributeValueFactory.FromDecimal(amount),
                            },
                        }
                    },
                    new TransactWriteItem
                    {
                        Put = CreateTransactionPut(customerId, transactionId, -amount, commission),
                    },
                }
            });

            return transactionId;
        }
        catch (TransactionCanceledException)
        {
            return null;
        }
        catch (ConditionalCheckFailedException)
        {
            return null;
        }
    }

    private string CreateTransctionId(string currency)
    {
        var timestampEpoch = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        return $"{currency}-{timestampEpoch}";
    }

    private Put CreateTransactionPut(string customerId, string transactionId, decimal amount, decimal commission)
    {
        return new Put
        {
            TableName = _tableName,
            Item =
            {
                ["CustomerId"] = AttributeValueFactory.FromString(customerId),
                ["TransactionId"] = AttributeValueFactory.FromString(transactionId),
                ["Amount"] = AttributeValueFactory.FromDecimal(amount),
                ["Commission"] = AttributeValueFactory.FromDecimal(commission),
            },
        };
    }

    private string ParseTransactionCurrency(string transactionId)
    {
        return transactionId.Substring(0, transactionId.IndexOf('-'));
    }

    private DateTime ParseTransactionTimestamp(string transactionId)
    {
        var timeString = transactionId.Substring(transactionId.IndexOf('-') + 1);
        var timeEpoch = long.Parse(timeString);
        var offset = DateTimeOffset.FromUnixTimeMilliseconds(timeEpoch);
        return offset.UtcDateTime;
    }
}
