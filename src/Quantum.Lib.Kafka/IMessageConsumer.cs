using System.Threading;
using System.Threading.Tasks;

namespace Quantum.Lib.Kafka;

public interface IMessageConsumer<TMessage>
{
    Task ConsumeAsync(string key, TMessage message, CancellationToken cancellationToken = default);
}
