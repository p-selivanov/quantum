namespace Quantum.Account.Api.Dtos;

public class WithdrawalCreateRequest
{
    public string Currency { get; set; }

    public decimal Amount { get; set; }
}
