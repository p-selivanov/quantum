namespace Quantum.AccountSearch.Projection.Events;

internal class CustomerCreated
{
    public string EmailAddress { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string PhoneNumber { get; set; }

    public string Country { get; set; }

    public string Status { get; set; }
}
