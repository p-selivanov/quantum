using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quantum.AccountSearch.Persistence;
using Quantum.AccountSearch.Projection.Events;
using Quantum.Lib.Kafka;

namespace Quantum.AccountSearch.Projection.Consumers;

internal class AccountBalanceUpdatedConsumer : IMessageConsumer<AccountBalanceUpdated>
{
    private readonly AccountSearchDbContext _dbContext;

    public AccountBalanceUpdatedConsumer(AccountSearchDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ConsumeAsync(Message<AccountBalanceUpdated> message, CancellationToken cancellationToken = default)
    {
        var dbCustomerAccount = await _dbContext.CustomerAccounts
            .FirstOrDefaultAsync(x => x.CustomerId == message.Key && x.Currency == message.Value.Currency);

        if (dbCustomerAccount is null)
        {
            var dbCustomer = await _dbContext.CustomerAccounts
                .FirstOrDefaultAsync(x => x.CustomerId == message.Key);

            dbCustomerAccount = dbCustomer.Clone();
            dbCustomerAccount.Currency = message.Value.Currency;

            _dbContext.CustomerAccounts.Add(dbCustomerAccount);

            if (string.IsNullOrEmpty(dbCustomer.Currency))
            {
                _dbContext.CustomerAccounts.Remove(dbCustomer);
            }
        }
        else if (dbCustomerAccount.Version >= message.Offset)
        {
            return;
        }

        dbCustomerAccount.Balance = message.Value.Balance;
        dbCustomerAccount.BalanceUpdatedAt = message.Timestamp;
        dbCustomerAccount.Version = message.Offset;

        if (message.Value.Balance > 0m &&
            dbCustomerAccount.FirstDepositTimestamp is null)
        {
            dbCustomerAccount.FirstDepositTimestamp = message.Timestamp;
        }

        await _dbContext.SaveChangesAsync();
    }
}
