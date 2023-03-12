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

    public async Task ConsumeAsync(Message<CustomerCreated> message, CancellationToken cancellationToken = default)
    {
        await _repository.CreateCustomerAsync(message.Key, message.Value.Country, message.Value.Status);
    }

    public async Task ConsumeAsync(Message<CustomerUpdated> message, CancellationToken cancellationToken = default)
    {
        if (message.Value.Status.IsSpecified ||
            message.Value.Country.IsSpecified)
        {
            await _repository.UpdateCustomerAsync(message.Key, message.Value.Country, message.Value.Status);
        }
    }
}
