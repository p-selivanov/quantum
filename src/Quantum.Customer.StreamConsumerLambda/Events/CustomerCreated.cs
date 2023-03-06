using System;

namespace Quantum.Customer.StreamConsumerLambda.Events;

internal class CustomerCreated
{
    public string Id { get; set; }

    public string EmailAddress { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string PhoneNumber { get; set; }

    public string Country { get; set; }

    public string Status { get; set; }

    public DateTime Timestamp { get; set; }
}
