using System.Threading;
using System.Threading.Tasks;

namespace Quantum.Lib.Kafka;

public interface IMessageConsumer<TValue>
{
    Task ConsumeAsync(Message<TValue> message, CancellationToken cancellationToken = default);
}
