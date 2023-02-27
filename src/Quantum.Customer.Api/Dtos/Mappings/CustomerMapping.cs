using Quantum.Customer.Models;

namespace Quantum.Customer.Api.Dtos.Mappings;

public static class CustomerMapping
{
    public static Customer ToDto(this CustomerDetail modelCustomer) => new()
    {
        Id = modelCustomer.Id,
        FirstName = modelCustomer.FirstName,
        LastName = modelCustomer.LastName,
        Status = modelCustomer.Status,
    };
}
