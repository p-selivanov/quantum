namespace Quantum.Account.Api.Models;

public class DepositCreateResult
{
    public string TransactionId { get; set; }

    public decimal AmountReceived { get; set; }

    public decimal AmountDeposited { get; set; }

    public decimal Commision { get; set; }
}
