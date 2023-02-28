using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Quantum.Customer.Api.Utils;
using Quantum.Customer.Models;

namespace Quantum.Customer.Repositories;

public class CustomerRepository
{
    private const string TableName = "Customers";

    private readonly IAmazonDynamoDB _client;

    public CustomerRepository(IAmazonDynamoDB client)
    {
        _client = client;
    }

    public async Task<CustomerDetail> GetCustomerAsync(string customerId)
    {
        var response = await _client.GetItemAsync(new GetItemRequest
        {
            TableName = TableName,
            Key =
            {
                ["Id"] = DynamoDbValue.FromString(customerId),
            },
        });

        if (response.IsItemSet == false)
        {
            return null;
        }

        var customer = new CustomerDetail
        {
            Id = response.Item["Id"].S,
            EmailAddress = response.Item["EmailAddress"].S,
            FirstName = response.Item["FirstName"].S,
            LastName = response.Item["LastName"].S,
            PhoneNumber = response.Item["PhoneNumber"].S,
            Country = response.Item["Country"].S,
            Status = response.Item["Status"].AsEnum<CustomerStatus>(),
            CreationTimestamp = response.Item["CreationTimestamp"].AsTimestamp(),
            UpdateTimestamp = response.Item["UpdateTimestamp"].AsTimestamp(),
        };

        return customer;
    }

    public async Task<string> CreateCustomerAsync(CustomerDetail customer)
    {
        var timestamp = DateTime.UtcNow;

        var response = await _client.PutItemAsync(new PutItemRequest
        {
            TableName = TableName,
            Item = new Dictionary<string, AttributeValue>
            {
                ["Id"] = DynamoDbValue.FromString(customer.Id),
                ["EmailAddress"] = DynamoDbValue.FromString(customer.EmailAddress),
                ["FirstName"] = DynamoDbValue.FromString(customer.FirstName),
                ["LastName"] = DynamoDbValue.FromString(customer.LastName),
                ["PhoneNumber"] = DynamoDbValue.FromString(customer.PhoneNumber),
                ["Country"] = DynamoDbValue.FromString(customer.Country),
                ["Status"] = DynamoDbValue.FromEnum(customer.Status),
                ["CreationTimestamp"] = DynamoDbValue.FromTimestamp(timestamp),
                ["UpdateTimestamp"] = DynamoDbValue.FromTimestamp(timestamp),
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
                TableName = TableName,
                Key =
                {
                    ["Id"] = DynamoDbValue.FromString(customerId),
                },
                UpdateExpression = "SET FirstName = :firstName, LastName = :lastName, UpdateTimestamp = :timestamp",
                ConditionExpression = "Id = :id",
                ExpressionAttributeValues =
                {
                    [":id"] = DynamoDbValue.FromString(customerId),
                    [":firstName"] = DynamoDbValue.FromString(firstName),
                    [":lastName"] = DynamoDbValue.FromString(lastName),
                    [":timestamp"] = DynamoDbValue.FromTimestamp(DateTime.UtcNow),
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
                TableName = TableName,
                Key =
                {
                    ["Id"] = DynamoDbValue.FromString(customerId),
                },
                UpdateExpression = "SET EmailAddress = :emailAddress, UpdateTimestamp = :timestamp",
                ConditionExpression = "Id = :id",
                ExpressionAttributeValues =
                {
                    [":id"] = DynamoDbValue.FromString(customerId),
                    [":emailAddress"] = DynamoDbValue.FromString(emailAddress),
                    [":timestamp"] = DynamoDbValue.FromTimestamp(DateTime.UtcNow),
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
                TableName = TableName,
                Key =
                {
                    ["Id"] = DynamoDbValue.FromString(customerId),
                },
                UpdateExpression = "SET PhoneNumber = :phoneNumber, UpdateTimestamp = :timestamp",
                ConditionExpression = "Id = :id",
                ExpressionAttributeValues =
                {
                    [":id"] = DynamoDbValue.FromString(customerId),
                    [":phoneNumber"] = DynamoDbValue.FromString(phoneNumber),
                    [":timestamp"] = DynamoDbValue.FromTimestamp(DateTime.UtcNow),
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
                TableName = TableName,
                Key =
                {
                    ["Id"] = DynamoDbValue.FromString(customerId),
                },
                UpdateExpression = "SET Country = :country, UpdateTimestamp = :timestamp",
                ConditionExpression = "Id = :id",
                ExpressionAttributeValues =
                {
                    [":id"] = DynamoDbValue.FromString(customerId),
                    [":country"] = DynamoDbValue.FromString(country),
                    [":timestamp"] = DynamoDbValue.FromTimestamp(DateTime.UtcNow),
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
