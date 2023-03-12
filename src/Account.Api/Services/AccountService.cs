using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Quantum.Acccount.Api.Models;
using Quantum.Account.Api.Configuration;
using Quantum.Account.Api.Models;
using Quantum.Account.Api.Repositories;
using Quantum.Lib.Common;

namespace Quantum.Account.Api.Services;

public class AccountService
{
    private readonly CustomerRepository _customerRepository;
    private readonly AccountRepository _accountRepository;
    private readonly TransactionRepository _transactionRepository;
    private readonly CommissionOptions _commissionOptions;

    public AccountService(
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

    public Task<List<AccountInfo>> GetCustomerAccountsAsync(string customerId)
    {
        return _accountRepository.GetAccountsAsync(customerId);
    }

    public Task<AccountInfo> GetCustomerAccountAsync(string customerId, string currency)
    {
        return _accountRepository.GetAccountAsync(customerId, currency);
    }

    public Task<List<TransactionInfo>> GetCustomerAccountTransactionsAsync(string customerId, string currency)
    {
        return _transactionRepository.GetTransactionsAsync(customerId, currency);
    }

    public async Task<OperationResult<DepositCreateResult>> DepositAsync(string customerId, string currency, decimal amount)
    {
        var customer = await _customerRepository.GetCustomerAsync(customerId);
        if (customer is null)
        {
            return OperationResult.Failure<DepositCreateResult>("Customer is not found", "404");
        }

        if (customer.Status == CustomerStatus.Suspended)
        {
            return OperationResult.Failure<DepositCreateResult>("The customer is suspended. Deposits are not allowed.");
        }

        var commission = 0m;
        var commissionPercent = ApplyCommisionDiscount(_commissionOptions.DepositCommisionPercent, customer.Country);
        if (commissionPercent > 0m)
        {
            commission = Math.Round(amount * commissionPercent / 100m, 8);
        }

        var transactionId = await _transactionRepository.CreateDepositTransactionAsync(customerId, currency, amount - commission, commission);
        if (transactionId is null)
        {
            throw new Exception("Deposit transaction canceled");
        }

        var result = new DepositCreateResult
        {
            TransactionId = transactionId,
            AmountReceived = amount,
            AmountDeposited = amount - commission,
            Commision = commission,
        };

        return result;
    }

    public async Task<OperationResult<WithdrawalCreateResult>> WithdrawAsync(string customerId, string currency, decimal amount)
    {
        var customer = await _customerRepository.GetCustomerAsync(customerId);
        if (customer is null)
        {
            return OperationResult.Failure<WithdrawalCreateResult>("Customer is not found", "404");
        }

        if (customer.Status == CustomerStatus.Suspended)
        {
            return OperationResult.Failure<WithdrawalCreateResult>("The customer is suspended. Withdrawals are not allowed.");
        }

        var commission = 0m;
        var commissionPercent = ApplyCommisionDiscount(_commissionOptions.WithdrawalCommisionPercent, customer.Country);
        if (_commissionOptions.WithdrawalCommisionPercent > 0m)
        {
            commission = Math.Round(amount * commissionPercent / 100m, 8);
        }

        var transactionId = await _transactionRepository.CreateWithdrawalTransactionAsync(customerId, currency, amount, commission);
        if (transactionId is null)
        {
            return OperationResult.Failure<WithdrawalCreateResult>($"{currency} account balance is not sufficient for withdrawal.");
        }

        var result = new WithdrawalCreateResult
        {
            TransactionId = transactionId,
            AmountSent = amount - commission,
            AmountWithdrawn = amount,
            Commision = commission,
        };

        return result;
    }

    private decimal ApplyCommisionDiscount(decimal commisionPercent, string country)
    {
        if (_commissionOptions.DiscountCountries?.Contains(country, StringComparer.OrdinalIgnoreCase) is true)
        {
            return commisionPercent / 2;
        }

        return commisionPercent;
    }
}
