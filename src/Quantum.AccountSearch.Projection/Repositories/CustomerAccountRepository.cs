using System;
using System.Threading.Tasks;
using Quantum.AccountSearch.Persistence;
using Quantum.AccountSearch.Persistence.Models;
using Quantum.AccountSearch.Projection.Events;

namespace Quantum.AccountSearch.Projection.Repositories;

internal class CustomerAccountRepository
{
    private readonly AccountSearchDbContext _dbContext;

    public CustomerAccountRepository(AccountSearchDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateCustomerAsync(string customerId, CustomerCreated @event, DateTime createdAt)
    {
        _dbContext.CustomerAccounts.Add(new CustomerAccount
        {
            CustomerId = customerId,
            Currency = string.Empty,
            EmailAddress = @event.EmailAddress,
            FirstName = @event.FirstName,
            LastName = @event.LastName,
            Country = @event.Country,
            Status = @event.Status,
            Balance = 0m,
            CustomerCreatedAt = createdAt,
        });

        await _dbContext.SaveChangesAsync();
    }
}
