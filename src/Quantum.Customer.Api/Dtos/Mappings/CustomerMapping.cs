using Quantum.Customer.Models;

namespace Quantum.Customer.Api.Dtos.Mappings;

public static class CustomerMapping
{
    public static Customer ToDto(this CustomerDetail model) => new()
    {
        Id = model.Id,
        EmailAddress = model.EmailAddress,
        FirstName = model.FirstName,
        LastName = model.LastName,
        PhoneNumber = model.PhoneNumber,
        Country = model.Country,
        Status = model.Status,
        CreatedAt = model.CreatedAt,
        UpdatedAt = model.UpdatedAt,
    };
}
