using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quantum.Customer.Api.Services;

public class CustomerService
{
    //public CustomerService(AmazonDynamoDBClient dbClient)
    //{
    //    _dbClient = dbClient;
    //}

    //public async Task<Dictionary<string, AttributeValue>> GetCustomerAsync(string customerId)
    //{
    //    var response = await _dbClient.GetItemAsync(new GetItemRequest
    //    {
    //        TableName = "Customers",
    //        Key = new Dictionary<string, AttributeValue>
    //        {
    //            ["Id"] = new AttributeValue(customerId),
    //        },
    //    });

    //    return response.Item;
    //}

    //public async Task<string> CreateCustomerAsync(string firstName, string lastName)
    //{
    //    var customer = new Models.Customer
    //    {
    //        Id = Guid.NewGuid().ToString(),
    //        FirstName = firstName,
    //        LastName = lastName,
    //        Status = Models.CustomerStatus.Lead,
    //    };

    //    var response = await _dbClient.PutItemAsync(new PutItemRequest
    //    {
    //        TableName = "Customers",
    //    });

    //    return customer.Id;
    //}

    //public async Task<OperationResult> UpdateCustomerTypeAsync(int customerId, CustomerStatus type)
    //{
    //    var customer = await this.dbContext.Customers
    //        .FirstOrDefaultAsync(x => x.Id == customerId);

    //    if (customer is null)
    //    {
    //        return OperationResult.NotFound();
    //    }

    //    if (customer.Type == type)
    //    {
    //        return OperationResult.Failure($"Customer type is already '{customer.Type}'.");
    //    }

    //    if (customer.Type == CustomerStatus.Lead &&
    //        type == CustomerStatus.Client)
    //    {
    //        customer.Type = type;
    //        customer.TradingAccountId = GenerateTradingAccountId();

    //        await this.dbContext.SaveChangesAsync();

    //        return OperationResult.Success();
    //    }

    //    return OperationResult.Failure($"Converting '{customer.Type}' to '{type}' is not allowed.");
    //}

    //public async Task<OperationResult> UpdateCustomerAgentAsync(int customerId, int? agentId)
    //{
    //    var customer = await this.dbContext.Customers
    //        .FirstOrDefaultAsync(x => x.Id == customerId);

    //    if (customer is null)
    //    {
    //        return OperationResult.NotFound();
    //    }

    //    if (agentId.HasValue)
    //    {
    //        //var agent = await GetAgentAsync(agentId.Value);
    //        //if (agent is null)
    //        //{
    //        //    return OperationResult.Failure($"Agent with ID {agentId} is not found.");
    //        //}

    //        customer.AgentId = agentId;
    //        await this.dbContext.SaveChangesAsync();

    //        //await NotifyAgentCustomerAssignedAsync(agent, customer.Id, customer.Name);
    //    }
    //    else
    //    {
    //        customer.AgentId = null;
    //        await this.dbContext.SaveChangesAsync();
    //    }

    //    return OperationResult.Success();
    //}

    //private string GenerateTradingAccountId()
    //{
    //    var rnd = new Random();
    //    return $"TA-{rnd.Next(10_000):0000}";
    //}
}
