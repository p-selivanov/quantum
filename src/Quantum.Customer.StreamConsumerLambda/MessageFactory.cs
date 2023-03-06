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
        var customerId = record.Dynamodb.Keys["Id"].AsString();
        var message = new Message<string, object>
        {
            Key = customerId,
        };

        if (record.EventName == OperationType.INSERT)
        {
            var createdEvent = ExtractCreatedEvent(record);
            message.Value = createdEvent;
            message.Timestamp = new Timestamp(createdEvent.Timestamp);
            return message;
        }
        else if (record.EventName == OperationType.MODIFY)
        {
            var updatedEvent = ExtractUpdatedEvent(record);
            message.Value = updatedEvent;
            message.Timestamp = new Timestamp(updatedEvent.Timestamp);
            return message;
        }

        return null;
    }

    private static CustomerCreated ExtractCreatedEvent(DynamoDBEvent.DynamodbStreamRecord record)
    {
        var item = record.Dynamodb.NewImage;
        return new CustomerCreated
        {
            Id = item["Id"].AsString(),
            EmailAddress = item["EmailAddress"].AsString(),
            FirstName = item["FirstName"].AsString(),
            LastName = item["LastName"].AsString(),
            PhoneNumber = item["PhoneNumber"].AsString(),
            Country = item["Country"].AsString(),
            Status = item["Status"].AsString(),
            Timestamp = item["CreatedAt"].AsTimestamp(),
        };
    }

    private static CustomerUpdated ExtractUpdatedEvent(DynamoDBEvent.DynamodbStreamRecord record)
    {
        var newItem = record.Dynamodb.NewImage;
        
        var @event = new CustomerUpdated
        {
            Id = newItem["Id"].AsString(),
            Timestamp = newItem["UpdatedAt"].AsTimestamp(),
        };

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

        return @event;
    }
}
