using Quantum.Customer.Api.Models;
using Quantum.Lib.Common;

namespace Quantum.Customer.Api.Dtos;

public class CustomerPatchRequest
{
    public Specifiable<string> EmailAddress { get; set; }

    public Specifiable<string> FirstName { get; set; }

    public Specifiable<string> LastName { get; set; }

    public Specifiable<string> Country { get; set; }

    public Specifiable<string> PhoneNumber { get; set; }

    public Specifiable<CustomerStatus> Status { get; set; }
}
