namespace Quantum.Account.Api.Configuration;

public class CommissionOptions
{
    public decimal DepositCommisionPercent { get; set; }

    public decimal WithdrawalCommisionPercent { get; set; }

    public string[] DiscountCountries { get; set; }
}
