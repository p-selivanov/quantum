using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Quantum.Acccount.Api.Models;
using Quantum.Account.Api.Configuration;
using Quantum.Account.Api.Dtos;
using Quantum.Account.Api.Dtos.Mappings;
using Quantum.Account.Api.Repositories;
using Quantum.Lib.AspNet;

namespace Quantum.Account.Api.Controllers;

[ApiController]
[Route("customers")]
public class CustomerAccountController : ControllerBase
{
    private readonly CustomerRepository _customerRepository;
    private readonly AccountRepository _accountRepository;
    private readonly TransactionRepository _transactionRepository;
    private readonly CommissionOptions _commissionOptions;

	public CustomerAccountController(
        CustomerRepository customerRepository,
        AccountRepository accountRepository,
        TransactionRepository transactionRepository,
        IOptions<CommissionOptions> commissionOptions)
	{
        _customerRepository = customerRepository;
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _commissionOptions = commissionOptions.Value;
    }

    [HttpGet("{customerId}/accounts")]
    public async Task<ActionResult<List<Dtos.Account>>> GetCustomerAccounts(string customerId)
    {
        var accounts = await _accountRepository.GetAccountsAsync(customerId);
        
        var accountDtos = accounts
            .Select(x => x.ToDto())
            .ToList();

        return Ok(accountDtos);
    }

    [HttpGet("{customerId}/accounts/{currency}")]
    public async Task<ActionResult<Dtos.Account>> GetCustomerAccounts(string customerId, string currency)
    {
        var account = await _accountRepository.GetAccountAsync(customerId, currency);
        if (account is null)
        {
            return NotFound();
        }

        var accountDto = account.ToDto();
        return Ok(accountDto);
    }

    [HttpGet("{customerId}/accounts/{currency}/transactions")]
    public async Task<ActionResult<List<Dtos.Transaction>>> GetCustomerAccountTransactions(string customerId, string currency)
    {
        var transactions = await _transactionRepository.GetTransactionsAsync(customerId, currency);

        var transactionDtos = transactions
            .Select(x => x.ToDto())
            .ToList();

        return Ok(transactionDtos);
    }

    [HttpPost("{customerId}/deposit")]
    public async Task<ActionResult<CustomerTransactionCreateResponse>> Deposit(string customerId, Dtos.CustomerTransactionCreateRequest request)
    {
        var customer = await _customerRepository.GetCustomerAsync(customerId);
        if (customer is null)
        {
            return NotFound();
        }

        if (customer.Status == CustomerStatus.Suspended)
        {
            return BadRequest(new ErrorResponse("The customer is suspended. Deposits are not allowed."));
        }

        var amount = request.Amount;
        var commission = 0m;
        if (_commissionOptions.DepositCommisionPercent > 0m)
        {
            commission = Math.Round(amount * _commissionOptions.DepositCommisionPercent / 100m, 8);
            amount -= commission;
        }

        var transactionId = await _transactionRepository.CreateDepositTransactionAsync(customerId, request.Currency, amount, commission);
        if (transactionId is null)
        {
            return StatusCode(500);
        }

        var response = new CustomerTransactionCreateResponse
        {
            TransactionId = transactionId,
            Amount = amount,
            Commision = commission,
        };

        return Ok(response);
    }

    [HttpPost("{customerId}/withdrawal")]
    public async Task<ActionResult> Withdraw(string customerId, Dtos.CustomerTransactionCreateRequest request)
    {
        var customer = await _customerRepository.GetCustomerAsync(customerId);
        if (customer is null)
        {
            return NotFound();
        }

        if (customer.Status == CustomerStatus.Suspended)
        {
            return BadRequest(new ErrorResponse("The customer is suspended. Withdrawals are not allowed."));
        }

        var amount = request.Amount;
        var commission = 0m;
        if (_commissionOptions.WithdrawalCommisionPercent > 0m)
        {
            commission = Math.Round(amount * _commissionOptions.WithdrawalCommisionPercent / 100m, 8);
            amount += commission;
        }

        var transactionId = await _transactionRepository.CreateWithdrawalTransactionAsync(customerId, request.Currency, amount, commission);
        if (transactionId is null)
        {
            return BadRequest(new ErrorResponse($"{request.Currency} account balance is not sufficient for withdrawal."));
        }

        var response = new CustomerTransactionCreateResponse
        {
            TransactionId = transactionId,
            Amount = -amount,
            Commision = commission,
        };

        return Ok(response);
    }
}
