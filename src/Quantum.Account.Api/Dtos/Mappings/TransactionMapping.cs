using Quantum.Account.Api.Models;

namespace Quantum.Account.Api.Dtos.Mappings;

internal static class TransactionMapping
{
    public static Transaction ToDto(this TransactionInfo model) => new()
    {
        Currency = model.Currency,
        Amount = model.Amount,
        Timestamp = model.Timestamp,
    };
}
