using System.Threading;
using System.Threading.Tasks;
using Quantum.AccountSearch.Projection.Events;
using Quantum.AccountSearch.Projection.Repositories;
using Quantum.Lib.Kafka;

namespace Quantum.AccountSearch.Projection;

internal class CustomerEventConsumer : IMessageConsumer<CustomerCreated>, IMessageConsumer<CustomerUpdated>
{
    private readonly CustomerAccountRepository _repository;

    public CustomerEventConsumer(CustomerAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task ConsumeAsync(Message<CustomerCreated> message, CancellationToken cancellationToken = default)
    {
        await _repository.CreateCustomerAsync(message.Key, message.Value, message.Timestamp);
    }

    public async Task ConsumeAsync(Message<CustomerUpdated> message, CancellationToken cancellationToken = default)
    {
        if (message.Value.Status.IsSpecified ||
            message.Value.Country.IsSpecified)
        {
            //await _repository.UpdateCustomerAsync(message.Key, message.Value.Country, message.Value.Status);
        }
    }
}
