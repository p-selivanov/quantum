using Amazon.DynamoDBv2;
using Amazon.Lambda.DynamoDBEvents;
using Confluent.Kafka;
using Quantum.Account.StreamConsumerLambda.Events;
using Quantum.Lib.DynamoDb;

namespace Quantum.Account.StreamConsumerLambda;

internal static class MessageFactory
{
    public static Message<string, object> ExtractMessage(DynamoDBEvent.DynamodbStreamRecord record)
    {
        if (record.EventName == OperationType.INSERT)
        {
            return ExtractCreatedEvent(record);
        }
        else if (record.EventName == OperationType.MODIFY)
        {
            return ExtractUpdatedEvent(record);
        }

        return null;
    }

    private static Message<string, object> ExtractCreatedEvent(DynamoDBEvent.DynamodbStreamRecord record)
    {
        var customerId = record.Dynamodb.Keys["CustomerId"].AsString();
        var fullTransactionId = record.Dynamodb.Keys["TransactionId"].AsString();
        var (currency, transactionId) = ParseTransactionId(fullTransactionId);

        if (string.IsNullOrEmpty(transactionId))
        {
            return null;
        }

        var item = record.Dynamodb.NewImage;
        
        return new Message<string, object>
        {
            Key = customerId,
            Value = new AccountTransactionCreated
            {
                Currency = currency,
                Amount = item["Amount"].AsDecimal(),
                Commission = item["Commission"].AsDecimal(),
            },
            Timestamp = new Timestamp(long.Parse(transactionId), TimestampType.CreateTime),
        };
    }

    private static Message<string, object> ExtractUpdatedEvent(DynamoDBEvent.DynamodbStreamRecord record)
    {
        var customerId = record.Dynamodb.Keys["CustomerId"].AsString();
        var fullTransactionId = record.Dynamodb.Keys["TransactionId"].AsString();
        var (currency, transactionId) = ParseTransactionId(fullTransactionId);

        if (string.IsNullOrEmpty(currency) ||
            string.IsNullOrEmpty(transactionId) is false)
        {
            return null;
        }

        var newItem = record.Dynamodb.NewImage;

        return new Message<string, object>
        {
            Key = customerId,
            Value = new AccountBalanceUpdated
            {
                Currency = currency,
                Balance = newItem["Balance"].AsDecimal(),
            },
            Timestamp = new Timestamp(newItem["UpdatedAt"].AsTimestamp()),
        };
    }

    private static (string, string) ParseTransactionId(string fullTransactionId)
    {
        if (string.IsNullOrEmpty(fullTransactionId))
        {
            return (null, null);
        }

        string currency;
        if (fullTransactionId.StartsWith('#'))
        {
            currency = fullTransactionId.Substring(1);
            return (currency, null);
        }

        var dashIndex = fullTransactionId.IndexOf('-');
        if (dashIndex == -1)
        {
            return (null, null);
        }

        currency = fullTransactionId.Substring(0, dashIndex);
        var id = fullTransactionId.Substring(dashIndex + 1);

        return (currency, id);
    }
}
