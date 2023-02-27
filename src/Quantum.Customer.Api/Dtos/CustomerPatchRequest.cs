using Quantum.Customer.Models;
using Quantum.Lib.Common;

namespace Quantum.Customer.Api.Dtos;

public class CustomerPatchRequest
{
    public Specifiable<CustomerStatus> Type { get; set; }

    public Specifiable<int?> AgentId { get; set; }
}
