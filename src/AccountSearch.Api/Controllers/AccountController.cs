using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quantum.AccountSearch.Api.Models;
using Quantum.AccountSearch.Api.Repositories;

namespace Quantum.AccountSearch.Api.Controllers;

[ApiController]
[Route("accounts")]
public class AccountController : ControllerBase
{
    private readonly CustomerAccountRepository _repository;

    public AccountController(CustomerAccountRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerAccountDetail>>> GetAccounts([FromQuery]CustomerAccountSearchRequest request)
    {
        var accounts = await _repository.SearchCustomerAccountsAsync(request);
        return Ok(accounts);
    }
}
