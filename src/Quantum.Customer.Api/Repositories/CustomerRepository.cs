using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
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
            Key = new Dictionary<string, AttributeValue>
            {
                ["Id"] = new AttributeValue(customerId),
            },
        });

        var customer = new CustomerDetail
        {
            Id = response.Item["Id"].S,
            FirstName = response.Item["FirstName"].S,
            LastName = response.Item["LastName"].S,
            //EmailAddress = response.Item["EmailAddress"].S,
            //PhoneNumber = response.Item["PhoneNumber"].S,
            //Status = response.Item["Status"].S,
        };

        return customer;
    }

    public async Task<string> CreateCustomerAsync(CustomerDetail customer)
    {
        await _client.PutItemAsync(new PutItemRequest
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
                ["CreationTimestamp"] = DynamoDbValue.FromTimestamp(customer.CreationTimestamp),
                ["UpdateTimestamp"] = DynamoDbValue.FromTimestamp(customer.UpdateTimestamp),
            },
        });

        return customer.Id;
    }
}
