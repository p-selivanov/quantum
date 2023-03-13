using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Options;
using Quantum.Customer.Api.Configuration;
using Quantum.Customer.Api.Models;
using Quantum.Lib.DynamoDb;

namespace Quantum.Customer.Repositories;

public class CustomerRepository
{
    private readonly IAmazonDynamoDB _client;
    private readonly string _tableName;

    public CustomerRepository(IAmazonDynamoDB client, IOptions<DynamoTableOptions> tableOptions)
    {
        _client = client;
        _tableName = tableOptions.Value.CustomerTableName;
    }

    public async Task<CustomerDetail> GetCustomerAsync(string customerId)
    {
        var response = await _client.GetItemAsync(new GetItemRequest
        {
            TableName = _tableName,
            Key =
            {
                ["Id"] = AttributeValueFactory.FromString(customerId),
            },
        });

        if (response.IsItemSet == false)
        {
            return null;
        }

        var customer = new CustomerDetail
        {
            Id = response.Item["Id"].AsString(),
            EmailAddress = response.Item["EmailAddress"].AsString(),
            FirstName = response.Item["FirstName"].AsString(),
            LastName = response.Item["LastName"].AsString(),
            PhoneNumber = response.Item["PhoneNumber"].AsString(),
            Country = response.Item["Country"].AsString(),
            Status = response.Item["Status"].AsEnum<CustomerStatus>(),
            CreatedAt = response.Item["CreatedAt"].AsTimestamp(),
            UpdatedAt = response.Item["UpdatedAt"].AsTimestamp(),
        };

        return customer;
    }

    public async Task<string> CreateCustomerAsync(CustomerDetail customer)
    {
        var timestamp = DateTime.UtcNow;

        var response = await _client.PutItemAsync(new PutItemRequest
        {
            TableName = _tableName,
            Item =
            {
                ["Id"] = AttributeValueFactory.FromString(customer.Id.ToLower()),
                ["EmailAddress"] = AttributeValueFactory.FromString(customer.EmailAddress.ToLower()),
                ["FirstName"] = AttributeValueFactory.FromString(customer.FirstName),
                ["LastName"] = AttributeValueFactory.FromString(customer.LastName),
                ["PhoneNumber"] = AttributeValueFactory.FromString(customer.PhoneNumber),
                ["Country"] = AttributeValueFactory.FromString(customer.Country),
                ["Status"] = AttributeValueFactory.FromEnum(customer.Status),
                ["CreatedAt"] = AttributeValueFactory.FromTimestamp(timestamp),
                ["UpdatedAt"] = AttributeValueFactory.FromTimestamp(timestamp),
            },
        });

        return customer.Id;
    }

    public async Task<bool> UpdateCustomerNameAsync(string customerId, string firstName, string lastName)
    {
        try
        {
            var response = await _client.UpdateItemAsync(new UpdateItemRequest
            {
                TableName = _tableName,
                Key =
                {
                    ["Id"] = AttributeValueFactory.FromString(customerId),
                },
                UpdateExpression = "SET FirstName = :firstName, LastName = :lastName, UpdatedAt = :timestamp",
                ConditionExpression = "Id = :id",
                ExpressionAttributeValues =
                {
                    [":id"] = AttributeValueFactory.FromString(customerId),
                    [":firstName"] = AttributeValueFactory.FromString(firstName),
                    [":lastName"] = AttributeValueFactory.FromString(lastName),
                    [":timestamp"] = AttributeValueFactory.FromTimestamp(DateTime.UtcNow),
                },
            });

            return true;
        }
        catch (ConditionalCheckFailedException)
        {
            return false;
        }
    }

    public async Task<bool> UpdateCustomerEmailAddressAsync(string customerId, string emailAddress)
    {
        try
        {
            var response = await _client.UpdateItemAsync(new UpdateItemRequest
            {
                TableName = _tableName,
                Key =
                {
                    ["Id"] = AttributeValueFactory.FromString(customerId),
                },
                UpdateExpression = "SET EmailAddress = :emailAddress, UpdatedAt = :timestamp",
                ConditionExpression = "Id = :id",
                ExpressionAttributeValues =
                {
                    [":id"] = AttributeValueFactory.FromString(customerId),
                    [":emailAddress"] = AttributeValueFactory.FromString(emailAddress.ToLower()),
                    [":timestamp"] = AttributeValueFactory.FromTimestamp(DateTime.UtcNow),
                },
            });

            return true;
        }
        catch (ConditionalCheckFailedException)
        {
            return false;
        }
    }

    public async Task<bool> UpdateCustomerPhoneNumberAsync(string customerId, string phoneNumber)
    {
        try
        {
            var response = await _client.UpdateItemAsync(new UpdateItemRequest
            {
                TableName = _tableName,
                Key =
                {
                    ["Id"] = AttributeValueFactory.FromString(customerId),
                },
                UpdateExpression = "SET PhoneNumber = :phoneNumber, UpdatedAt = :timestamp",
                ConditionExpression = "Id = :id",
                ExpressionAttributeValues =
                {
                    [":id"] = AttributeValueFactory.FromString(customerId),
                    [":phoneNumber"] = AttributeValueFactory.FromString(phoneNumber),
                    [":timestamp"] = AttributeValueFactory.FromTimestamp(DateTime.UtcNow),
                },
            });

            return true;
        }
        catch (ConditionalCheckFailedException)
        {
            return false;
        }
    }

    public async Task<bool> UpdateCustomerCountryAsync(string customerId, string country)
    {
        try
        {
            var response = await _client.UpdateItemAsync(new UpdateItemRequest
            {
                TableName = _tableName,
                Key =
                {
                    ["Id"] = AttributeValueFactory.FromString(customerId),
                },
                UpdateExpression = "SET Country = :country, UpdatedAt = :timestamp",
                ConditionExpression = "Id = :id",
                ExpressionAttributeValues =
                {
                    [":id"] = AttributeValueFactory.FromString(customerId),
                    [":country"] = AttributeValueFactory.FromString(country),
                    [":timestamp"] = AttributeValueFactory.FromTimestamp(DateTime.UtcNow),
                },
            });

            return true;
        }
        catch (ConditionalCheckFailedException)
        {
            return false;
        }
    }

    public async Task<bool> UpdateCustomerStatusAsync(string customerId, CustomerStatus status)
    {
        try
        {
            var response = await _client.UpdateItemAsync(new UpdateItemRequest
            {
                TableName = _tableName,
                Key =
                {
                    ["Id"] = AttributeValueFactory.FromString(customerId),
                },
                UpdateExpression = "SET #status = :status, UpdatedAt = :timestamp",
                ConditionExpression = "Id = :id",
                ExpressionAttributeValues =
                {
                    [":id"] = AttributeValueFactory.FromString(customerId),
                    [":status"] = AttributeValueFactory.FromEnum(status),
                    [":timestamp"] = AttributeValueFactory.FromTimestamp(DateTime.UtcNow),
                },
                ExpressionAttributeNames =
                {
                    ["#status"] = "Status",
                },
            });

            return true;
        }
        catch (ConditionalCheckFailedException)
        {
            return false;
        }
    }
}
