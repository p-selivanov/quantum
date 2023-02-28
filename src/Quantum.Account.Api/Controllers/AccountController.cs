using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quantum.Account.Api.Repositories;
using Quantum.Lib.AspNet;

namespace Quantum.Account.Api.Controllers;

[ApiController]
[Route("accounts")]
public class AccountController : ControllerBase
{
    private readonly AccountRepository _accountRepository;
    private readonly TransactionRepository _transactionRepository;

	public AccountController(AccountRepository accountRepository, TransactionRepository transactionRepository)
	{
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
	}

    [HttpGet("{accountId}")]
    public async Task<ActionResult<Dtos.Account>> GetAccount(string accountId)
    {
        var account = await _accountRepository.GetAccount(accountId);
        return Ok(account);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAccount(Dtos.AccountCreateRequest request)
    {
        var accountId = Guid.NewGuid().ToString();
        await _accountRepository.CreateAccount(accountId, request.CustomerId, request.Currency);

        return CreatedAtAction(nameof(GetAccount), new { accountId }, new { accountId });
    }

    [HttpPost("{accountId}/deposit")]
    public async Task<ActionResult> Deposit(string accountId, Dtos.TransactionCreateRequest request)
    {
        await _transactionRepository.CreateTransactionAsync(accountId, request.Amount);

        return NoContent();
    }

    [HttpPost("{accountId}/withdrawal")]
    public async Task<ActionResult> Withdraw(string accountId, Dtos.TransactionCreateRequest request)
    {
        var result = await _transactionRepository.CreateTransactionAsync(accountId, -request.Amount);
        if (result == false)
        {
            return BadRequest(new ErrorResponse($"Balance is not sufficient to withdraw {request.Amount}"));
        }

        return NoContent();
    }
}
