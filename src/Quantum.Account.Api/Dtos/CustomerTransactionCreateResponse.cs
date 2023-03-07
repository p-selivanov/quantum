namespace Quantum.Account.Api.Dtos;

public class CustomerTransactionCreateResponse
{
    public string TransactionId { get; set; }

    public decimal Amount { get; set; }

    public decimal Commision { get; set; }
}
