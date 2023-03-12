namespace Quantum.Account.StreamConsumerLambda.Events;

internal class AccountBalanceUpdated
{
    public string Currency { get; set; }

    public decimal Balance { get; set; }
}
