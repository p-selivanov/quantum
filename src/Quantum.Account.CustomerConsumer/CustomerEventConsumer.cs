using System.Threading;
using System.Threading.Tasks;
using Quantum.Account.CustomerConsumer.Events;
using Quantum.Account.CustomerConsumer.Repositories;
using Quantum.Lib.Kafka;

namespace Quantum.Account.CustomerConsumer;

internal class CustomerEventConsumer : IMessageConsumer<CustomerCreated>, IMessageConsumer<CustomerUpdated>
{
    private readonly CustomerRepository _repository;

    public CustomerEventConsumer(CustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task ConsumeAsync(string key, CustomerCreated message, CancellationToken cancellationToken = default)
    {
        await _repository.CreateCustomerAsync(key, message.Country, message.Status);
    }

    public async Task ConsumeAsync(string key, CustomerUpdated message, CancellationToken cancellationToken = default)
    {
        if (message.Status.IsSpecified ||
            message.Country.IsSpecified)
        {
            await _repository.UpdateCustomerAsync(key, message.Country, message.Status);
        }
    }
}
