using Amazon.DynamoDBv2;
using Amazon.Lambda.DynamoDBEvents;
using Confluent.Kafka;
using Quantum.Customer.StreamConsumerLambda.Events;
using Quantum.Customer.StreamConsumerLambda.Utils;
using Quantum.Lib.DynamoDb;

namespace Quantum.Customer.StreamConsumerLambda;

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
        var customerId = record.Dynamodb.Keys["Id"].AsString();
        var item = record.Dynamodb.NewImage;
        
        return new Message<string, object>
        {
            Key = customerId,
            Value = new CustomerCreated
            {
                EmailAddress = item["EmailAddress"].AsString(),
                FirstName = item["FirstName"].AsString(),
                LastName = item["LastName"].AsString(),
                PhoneNumber = item["PhoneNumber"].AsString(),
                Country = item["Country"].AsString(),
                Status = item["Status"].AsString(),
            },
            Timestamp = new Timestamp(item["CreatedAt"].AsTimestamp()),
        };
    }

    private static Message<string, object> ExtractUpdatedEvent(DynamoDBEvent.DynamodbStreamRecord record)
    {
        var customerId = record.Dynamodb.Keys["Id"].AsString();
        var newItem = record.Dynamodb.NewImage;

        var @event = new CustomerUpdated();

        if (record.IsAttributeChanged("EmailAddress"))
        {
            @event.EmailAddress = newItem["EmailAddress"].AsString();
        }

        if (record.IsAttributeChanged("FirstName"))
        {
            @event.FirstName = newItem["FirstName"].AsString();
        }

        if (record.IsAttributeChanged("LastName"))
        {
            @event.LastName = newItem["LastName"].AsString();
        }

        if (record.IsAttributeChanged("PhoneNumber"))
        {
            @event.PhoneNumber = newItem["PhoneNumber"].AsString();
        }

        if (record.IsAttributeChanged("Country"))
        {
            @event.Country = newItem["Country"].AsString();
        }

        if (record.IsAttributeChanged("Status"))
        {
            @event.Status = newItem["Status"].AsString();
        }

        return new Message<string, object>
        {
            Key = customerId,
            Value = @event,
            Timestamp = new Timestamp(newItem["UpdatedAt"].AsTimestamp()),
        };
    }
}
