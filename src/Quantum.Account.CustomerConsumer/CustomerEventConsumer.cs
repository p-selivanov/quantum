using System;
using System.Threading;
using System.Threading.Tasks;
using Quantum.Account.CustomerConsumer.Events;
using Quantum.Lib.Kafka;

namespace Quantum.Account.CustomerConsumer;

internal class CustomerEventConsumer : IMessageConsumer<CustomerCreated>, IMessageConsumer<CustomerUpdated>
{
    public CustomerEventConsumer()
    {
    }

    public Task ConsumeAsync(string key, CustomerCreated message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task ConsumeAsync(string key, CustomerUpdated message, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
