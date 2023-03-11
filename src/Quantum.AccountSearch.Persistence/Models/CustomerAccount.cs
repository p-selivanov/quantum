using System;

namespace Quantum.AccountSearch.Persistence.Models;

public class CustomerAccount
{
    public string CustomerId { get; set; }

    public string Currency { get; set; }

    public string EmailAddress { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Country { get; set; }

    public string Status { get; set; }

    public decimal Balance { get; set; }

    public DateTime CustomerCreatedAt { get; set; }

    public DateTime? BalanceUpdatedAt { get; set; }

    public bool HadSuspension { get; set; }

    public DateTime? FirstDepositTimestamp { get; set; }

    //public string VerificationLevel { get; set; }

    //public string Category { get; set; }

    //public string[] Tags { get; set; }

    public long Version { get; set; }

    public CustomerAccount Clone()
    {
        return (CustomerAccount)MemberwiseClone();
    }
}
