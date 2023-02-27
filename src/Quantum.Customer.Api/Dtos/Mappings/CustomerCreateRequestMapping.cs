using Quantum.Customer.Models;

namespace Quantum.Customer.Api.Dtos.Mappings;

public static class CustomerCreateRequestMapping
{
    public static CustomerDetail ToModel(this CustomerCreateRequest dto)
    {
        return new CustomerDetail
        {
            EmailAddress = dto.EmailAddress,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            Country = dto.Country,
        };
    }
}
