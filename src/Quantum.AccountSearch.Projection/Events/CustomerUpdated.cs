using Quantum.Lib.Common;

namespace Quantum.AccountSearch.Projection.Events;

internal class CustomerUpdated
{
    public Specifiable<string> Country { get; set; }

    public Specifiable<string> Status { get; set; }
}
