using Quantum.Lib.Common;

namespace Quantum.Customer.StreamConsumerLambda.Events;

internal class CustomerUpdated
{
    public Specifiable<string> EmailAddress { get; set; }

    public Specifiable<string> FirstName { get; set; }

    public Specifiable<string> LastName { get; set; }

    public Specifiable<string> PhoneNumber { get; set; }

    public Specifiable<string> Country { get; set; }

    public Specifiable<string> Status { get; set; }
}
