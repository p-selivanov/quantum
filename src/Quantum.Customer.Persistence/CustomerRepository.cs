using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Quantum.Customer.Persistence.Models;

namespace Quantum.Customer.Persistence;

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
                ["Id"] = new AttributeValue(customer.Id),
                ["EmailAddress"] = new AttributeValue(customer.EmailAddress),
                ["FirstName"] = new AttributeValue(customer.FirstName),
                ["LastName"] = new AttributeValue(customer.LastName),
                ["PhoneNumber"] = string.IsNullOrEmpty(customer.PhoneNumber) == false ? new AttributeValue(customer.PhoneNumber) : new AttributeValue
                {
                    NULL = true
                },
                //["Country"] = new AttributeValue(customer.Country),
                //["Status"] = new AttributeValue(customer.Status.ToString()),
                //["CreationTimestamp"] = new AttributeValue(
                //    ((DateTimeOffset)customer.CreationTimestamp).ToUnixTimeMilliseconds().ToString()),
                //["UpdateTimestamp"] = new AttributeValue(
                //    ((DateTimeOffset)customer.UpdateTimestamp).ToUnixTimeMilliseconds().ToString()),
            },
        });

        return customer.Id;
    }
}
