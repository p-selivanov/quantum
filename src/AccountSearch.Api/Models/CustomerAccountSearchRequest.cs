namespace Quantum.AccountSearch.Api.Models;

public class CustomerAccountSearchRequest
{
    public string CustomerId { get; set; }

    public string Currency { get; set; }

    public string EmailAddress { get; set; }

    public string Name { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Country { get; set; }

    public string Status { get; set; }

    public decimal? MinBalance { get; set; }

    public decimal? MaxBalance { get; set; }


    public string Sort { get; set; }

    public bool Desc { get; set; }


    public int Offset { get; set; }

    public int Limit { get; set; }
}
