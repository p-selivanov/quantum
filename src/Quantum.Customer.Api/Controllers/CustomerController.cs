using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quantum.Customer.Api.Dtos.Mappings;
using Quantum.Customer.Repositories;

namespace Quantum.Customer.Api.Controllers;

[ApiController]
[Route("customers")]
public class CustomerController : ControllerBase
{
    private readonly CustomerRepository _customerRepository;

    public CustomerController(CustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<Dtos.Customer>>> GetCustomer(string id)
    {
        var customer = await _customerRepository.GetCustomerAsync(id);

        if (customer is null)
        {
            return NotFound();
        }

        var customerDto = customer.ToDto();

        return Ok(customerDto);
    }

    [HttpPost()]
    public async Task<ActionResult> CreateCustomer(Dtos.CustomerCreateRequest request)
    {
        var customerDetail = request.ToModel();
        customerDetail.Id = Guid.NewGuid().ToString();
        customerDetail.CreationTimestamp = DateTime.UtcNow;
        customerDetail.UpdateTimestamp = DateTime.UtcNow;

        var customerId = await _customerRepository.CreateCustomerAsync(customerDetail);

        return CreatedAtAction(nameof(GetCustomer), new { id = customerId }, new { id = customerId });
    }

    //[HttpPatch("{id}")]
    //public async Task<ActionResult> PatchCustomer(int id, Dtos.CustomerPatchRequest request)
    //{
    //    if (request.Type.IsSpecified)
    //    {
    //        var result = await this.customerService.UpdateCustomerTypeAsync(id, request.Type.Value);
    //        if (result.IsFailure)
    //        {
    //            return result.ToActionResult();
    //        }
    //    }

    //    if (request.AgentId.IsSpecified)
    //    {
    //        var result = await this.customerService.UpdateCustomerAgentAsync(id, request.AgentId.Value);
    //        if (result.IsFailure)
    //        {
    //            return result.ToActionResult();
    //        }
    //    }

    //    return NoContent();
    //}
}
