namespace Quantum.Account.Api.Models;

public class WithdrawalCreateResult
{
    public string TransactionId { get; set; }

    public decimal AmountSent { get; set; }

    public decimal AmountWithdrawn { get; set; }

    public decimal Commision { get; set; }
}
