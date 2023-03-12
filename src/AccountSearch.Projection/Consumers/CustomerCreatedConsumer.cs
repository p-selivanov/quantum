using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quantum.AccountSearch.Persistence;
using Quantum.AccountSearch.Persistence.Models;
using Quantum.AccountSearch.Projection.Events;
using Quantum.Lib.Kafka;

namespace Quantum.AccountSearch.Projection.Consumers;

internal class CustomerCreatedConsumer : IMessageConsumer<CustomerCreated>
{
    private readonly AccountSearchDbContext _dbContext;

    public CustomerCreatedConsumer(AccountSearchDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ConsumeAsync(Message<CustomerCreated> message, CancellationToken cancellationToken = default)
    {
        var customerExists = await _dbContext.CustomerAccounts.AnyAsync(x => x.CustomerId == message.Key);
        if (customerExists)
        {
            return;
        }

        _dbContext.CustomerAccounts.Add(new CustomerAccount
        {
            CustomerId = message.Key,
            Currency = string.Empty,
            EmailAddress = message.Value.EmailAddress,
            FirstName = message.Value.FirstName,
            LastName = message.Value.LastName,
            Country = message.Value.Country,
            Status = message.Value.Status,
            Balance = 0m,
            CustomerCreatedAt = message.Timestamp,
            Version = message.Offset,
        });

        await _dbContext.SaveChangesAsync();
    }
}
