using Quantum.Customer.Models;

namespace Quantum.Customer.Api.Dtos;

public class Customer
{
    public string Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public CustomerStatus Status { get; set; }
}
