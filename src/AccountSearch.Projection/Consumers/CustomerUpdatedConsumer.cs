using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quantum.AccountSearch.Persistence;
using Quantum.AccountSearch.Persistence.Models;
using Quantum.AccountSearch.Projection.Events;
using Quantum.AccountSearch.Projection.Utils;
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
        var expr = ExecuteUpdateExpression.Empty<CustomerAccount>();
        var ev = message.Value;

        if (ev.EmailAddress.IsSpecified)
        {
            expr = expr.SetPropertyEx(x => x.EmailAddress, ev.EmailAddress.Value);
        }

        if (ev.FirstName.IsSpecified)
        {
            expr = expr.SetPropertyEx(x => x.FirstName, ev.FirstName.Value);
        }

        if (ev.LastName.IsSpecified)
        {
            expr = expr.SetPropertyEx(x => x.LastName, ev.LastName.Value);
        }

        if (ev.Country.IsSpecified)
        {
            expr = expr.SetPropertyEx(x => x.Country, ev.Country.Value);
        }

        if (ev.Status.IsSpecified)
        {
            expr = expr.SetPropertyEx(x => x.Status, ev.Status.Value);
        }

        if (expr.IsEmpty())
        {
            return;
        }

        expr = expr.SetPropertyEx(x => x.Version, message.Offset);

        await _dbContext.CustomerAccounts
            .Where(x => x.CustomerId == message.Key && x.Version < message.Offset)
            .ExecuteUpdateAsync(expr);
    }
}
