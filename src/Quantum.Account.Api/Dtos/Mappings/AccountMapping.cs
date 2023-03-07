using Quantum.Account.Api.Models;

namespace Quantum.Account.Api.Dtos.Mappings;

internal static class AccountMapping
{
    public static Account ToDto(this AccountInfo model) => new()
    {
        Currency = model.Currency,
        Balance = model.Balance,
    };
}
