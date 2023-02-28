using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quantum.Customer.Api.Dtos.Mappings;
using Quantum.Customer.Api.Services;
using Quantum.Lib.AspNet;

namespace Quantum.Customer.Api.Controllers;

[ApiController]
[Route("customers")]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomerController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Dtos.Customer>> GetCustomer(string id)
    {
        var customer = await _customerService.GetCustomerAsync(id);
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
        var customerId = await _customerService.CreateCustomerAsync(customerDetail);

        return CreatedAtAction(nameof(GetCustomer), new { id = customerId }, new { id = customerId });
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult> PatchCustomer(string id, Dtos.CustomerPatchRequest request)
    {
        if (request.EmailAddress.IsSpecified)
        {
            await _customerService.UpdateCustomerEmailAddressAsync(id, request.EmailAddress.Value);
        }

        if (request.FirstName.IsSpecified ||
            request.LastName.IsSpecified)
        {
            await _customerService.UpdateCustomerNameAsync(id, request.FirstName.Value, request.LastName.Value);
        }

        if (request.PhoneNumber.IsSpecified)
        {
            await _customerService.UpdateCustomerPhoneNumberAsync(id, request.PhoneNumber.Value);
        }

        if (request.Country.IsSpecified)
        {
            var result = await _customerService.UpdateCustomerCountryAsync(id, request.Country.Value);
            if (result.IsFailure)
            {
                return result.ToActionResult();
            }
        }

        return NoContent();
    }
}
