using Quantum.Acccount.Api.Models;

namespace Quantum.Account.Api.Models;

public class CustomerInfo
{
    public string Id { get; set; }

    public string Country { get; set; }

    public CustomerStatus Status { get; set; }
}
