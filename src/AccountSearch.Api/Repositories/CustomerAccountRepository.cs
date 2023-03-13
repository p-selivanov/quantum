using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quantum.AccountSearch.Api.Models;
using Quantum.AccountSearch.Persistence;
using Quantum.AccountSearch.Persistence.Models;

namespace Quantum.AccountSearch.Api.Repositories;

public class CustomerAccountRepository
{
    private const int DefaultLimit = 20;
    private const int MaxLimit = 100;

    private readonly AccountSearchDbContext _dbContext;

    public CustomerAccountRepository(AccountSearchDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<CustomerAccountDetail>> SearchCustomerAccountsAsync(CustomerAccountSearchRequest request)
    {
        IQueryable<CustomerAccount> query = _dbContext.CustomerAccounts;

        query = ApplyFilter(query, request);
        query = ApplySorting(query, request.Sort, request.Desc);
        query = ApplyOffsetLimit(query, request.Offset, request.Limit);

        var items = await query
            .Select(x => new CustomerAccountDetail
            {
                CustomerId = x.CustomerId,
                EmailAddress = x.EmailAddress,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Country = x.Country,
                Status = x.Status,
                Currency = x.Currency,
                Balance = x.Balance,
                CustomerCreatedAt = x.CustomerCreatedAt,
                BalanceUpdatedAt = x.BalanceUpdatedAt,
                FirstDepositTimestamp = x.FirstDepositTimestamp,
                HadSuspension = x.HadSuspension,
            })
            .ToListAsync();

        return items;
    }

    private IQueryable<CustomerAccount> ApplyFilter(IQueryable<CustomerAccount> query, CustomerAccountSearchRequest request)
    {
        if (string.IsNullOrEmpty(request.CustomerId) is false)
        {
            query = query.Where(x => x.CustomerId.StartsWith(request.CustomerId.ToLower()));
        }

        if (string.IsNullOrEmpty(request.Currency) is false)
        {
            query = query.Where(x => x.Currency.StartsWith(request.Currency.ToUpper()));
        }
        else
        {
            query = query.Where(x => x.Currency != string.Empty);
        }

        if (string.IsNullOrEmpty(request.EmailAddress) is false)
        {
            query = query.Where(x => x.EmailAddress.StartsWith(request.EmailAddress.ToLower()));
        }

        if (string.IsNullOrEmpty(request.Name) is false)
        {
            query = query.Where(x => x.FirstName.StartsWith(request.Name) || x.LastName.StartsWith(request.Name));
        }

        if (string.IsNullOrEmpty(request.FirstName) is false)
        {
            query = query.Where(x => x.FirstName.StartsWith(request.FirstName));
        }

        if (string.IsNullOrEmpty(request.LastName) is false)
        {
            query = query.Where(x => x.LastName.StartsWith(request.LastName));
        }

        if (string.IsNullOrEmpty(request.Country) is false)
        {
            query = query.Where(x => x.Country.StartsWith(request.Country));
        }

        if (string.IsNullOrEmpty(request.Status) is false)
        {
            query = query.Where(x => x.Status.StartsWith(request.Status));
        }

        if (request.MinBalance is not null)
        {
            query = query.Where(x => x.Balance >= request.MinBalance);
        }

        if (request.MaxBalance is not null)
        {
            query = query.Where(x => x.Balance <= request.MaxBalance);
        }

        return query;
    }

    private IQueryable<CustomerAccount> ApplySorting(IQueryable<CustomerAccount> query, string sort, bool desc)
    {
        Dictionary<string, Expression<Func<CustomerAccount, object>>> sortSelectors = new(StringComparer.OrdinalIgnoreCase)
        {
            [nameof(CustomerAccount.EmailAddress)] = x => x.EmailAddress,
            [nameof(CustomerAccount.FirstName)] = x => x.FirstName,
            [nameof(CustomerAccount.LastName)] = x => x.LastName,
            [nameof(CustomerAccount.Country)] = x => x.Country,
            [nameof(CustomerAccount.Balance)] = x => x.Balance,
            [nameof(CustomerAccount.CustomerCreatedAt)] = x => x.CustomerCreatedAt,
            [nameof(CustomerAccount.BalanceUpdatedAt)] = x => x.BalanceUpdatedAt,
        };

        if (string.IsNullOrEmpty(sort) is false &&
            sortSelectors.TryGetValue(sort, out var selector))
        {
            return desc ? query.OrderByDescending(selector) : query.OrderBy(selector);
        }

        return query.OrderBy(x => x.CustomerId).ThenBy(x => x.Currency);
    }

    private IQueryable<CustomerAccount> ApplyOffsetLimit(IQueryable<CustomerAccount> query, int offset, int limit)
    {
        if (offset > 0)
        {
            query = query.Skip(offset);
        }

        if (limit <= 0)
        {
            limit = DefaultLimit;
        }
        else if (limit > MaxLimit)
        {
            limit = MaxLimit;
        }

        query = query.Take(limit);

        return query;
    }
}
