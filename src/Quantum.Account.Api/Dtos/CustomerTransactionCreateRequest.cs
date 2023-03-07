namespace Quantum.Account.Api.Dtos;

public class CustomerTransactionCreateRequest
{
    public string Currency { get; set; }

    public decimal Amount { get; set; }
}
