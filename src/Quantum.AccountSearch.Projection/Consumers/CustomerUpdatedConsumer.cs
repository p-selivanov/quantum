using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Quantum.AccountSearch.Persistence;
using Quantum.AccountSearch.Persistence.Models;
using Quantum.AccountSearch.Projection.Events;
using Quantum.Lib.Kafka;

namespace Quantum.AccountSearch.Projection.Consumers;

internal class CustomerUpdatedConsumer : IMessageConsumer<CustomerUpdated>
{
    private readonly AccountSearchDbContext _dbContext;

    public CustomerUpdatedConsumer(AccountSearchDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ConsumeAsync(Message<CustomerUpdated> message, CancellationToken cancellationToken = default)
    {
        Expression<Func<SetPropertyCalls<CustomerAccount>, SetPropertyCalls<CustomerAccount>>> expr = x => x;
        var @event = message.Value;
        var hasUpdates = false;

        if (@event.EmailAddress.IsSpecified)
        {
            expr = calls => calls.SetProperty(x => x.EmailAddress, @event.EmailAddress.Value);
            hasUpdates = true;
        }

        if (@event.FirstName.IsSpecified)
        {
            expr = calls => calls.SetProperty(x => x.FirstName, @event.FirstName.Value);
            hasUpdates = true;
        }

        if (@event.LastName.IsSpecified)
        {
            expr = calls => calls.SetProperty(x => x.LastName, @event.LastName.Value);
            hasUpdates = true;
        }

        if (@event.Country.IsSpecified)
        {
            expr = calls => calls.SetProperty(x => x.Country, @event.Country.Value);
            hasUpdates = true;
        }

        if (@event.Status.IsSpecified)
        {
            expr = calls => calls.SetProperty(x => x.Status, @event.Status.Value);
            hasUpdates = true;
        }

        if (hasUpdates is false)
        {
            return;
        }

        await _dbContext.CustomerAccounts
            .Where(x => x.CustomerId == message.Key)
            .ExecuteUpdateAsync(expr);
    }
}
