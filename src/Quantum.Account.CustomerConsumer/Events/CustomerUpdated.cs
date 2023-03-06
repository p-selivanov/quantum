using Quantum.Lib.Common;

namespace Quantum.Account.CustomerConsumer.Events;

internal class CustomerUpdated
{
    public Specifiable<string> Country { get; set; }

    public Specifiable<string> Status { get; set; }
}
