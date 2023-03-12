namespace Quantum.Customer.Api.Dtos;

public class CustomerCreateRequest
{
    public string EmailAddress { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Country { get; set; }

    public string PhoneNumber { get; set; }
}
