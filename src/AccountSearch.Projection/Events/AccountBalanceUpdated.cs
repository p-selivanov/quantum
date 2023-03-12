namespace Quantum.AccountSearch.Projection.Events;

internal class AccountBalanceUpdated
{
    public string Currency { get; set; }

    public decimal Balance { get; set; }
}
