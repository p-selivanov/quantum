namespace Quantum.Account.Api.Dtos;

public class DepositCreateRequest
{
    public string Currency { get; set; }

    public decimal Amount { get; set; }
}
