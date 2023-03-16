using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quantum.Account.Api.Dtos;
using Quantum.Account.Api.Models;
using Quantum.Account.Api.Services;
using Quantum.Lib.AspNet;

namespace Quantum.Account.Api.Controllers;

[ApiController]
[Route("customers")]
public class CustomerAccountController : ControllerBase
{
    private readonly AccountService _accountService;

	public CustomerAccountController(AccountService accountService)
	{
        _accountService = accountService;
    }

    [HttpGet("{customerId}/accounts")]
    public async Task<ActionResult<List<AccountInfo>>> GetCustomerAccounts(string customerId)
    {
        var accounts = await _accountService.GetCustomerAccountsAsync(customerId);
        return Ok(accounts);
    }

    [HttpGet("{customerId}/accounts/{currency}")]
    public async Task<ActionResult<AccountInfo>> GetCustomerAccount(string customerId, string currency)
    {
        var account = await _accountService.GetCustomerAccountAsync(customerId, currency);
        if (account is null)
        {
            return NotFound();
        }

        return Ok(account);
    }

    [HttpGet("{customerId}/accounts/{currency}/transactions")]
    public async Task<ActionResult<List<TransactionInfo>>> GetCustomerAccountTransactions(string customerId, string currency)
    {
        var transactions = await _accountService.GetCustomerAccountTransactionsAsync(customerId, currency);
        return Ok(transactions);
    }

    [HttpPost("{customerId}/deposits")]
    public async Task<ActionResult<DepositCreateResult>> Deposit(string customerId, DepositCreateRequest request)
    {
        var result = await _accountService.DepositAsync(customerId, request.Currency, request.Amount);
        if (result.IsFailure)
        {
            return result.ToActionResult();
        }

        return Ok(result.Value);
    }

    [HttpPost("{customerId}/withdrawals")]
    public async Task<ActionResult<WithdrawalCreateResult>> Withdraw(string customerId, WithdrawalCreateRequest request)
    {
        var result = await _accountService.WithdrawAsync(customerId, request.Currency, request.Amount);
        if (result.IsFailure)
        {
            return result.ToActionResult();
        }

        return Ok(result.Value);
    }
}
