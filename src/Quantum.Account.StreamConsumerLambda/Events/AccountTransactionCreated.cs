namespace Quantum.Account.StreamConsumerLambda.Events;

internal class AccountTransactionCreated
{
    public string Currency { get; set; }

    public decimal Amount { get; set; }

    public decimal Commission { get; set; }
}
