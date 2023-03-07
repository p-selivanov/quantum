using System;
using System.Threading.Tasks;
using Quantum.Customer.Api.Models;
using Quantum.Customer.Repositories;
using Quantum.Lib.Common;

namespace Quantum.Customer.Api.Services;

public class CustomerService
{
    private readonly CustomerRepository _customerRepository;

    public CustomerService(CustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public Task<CustomerDetail> GetCustomerAsync(string customerId)
    {
        return _customerRepository.GetCustomerAsync(customerId);
    }

    public async Task<string> CreateCustomerAsync(CustomerDetail customer)
    {
        customer.Id = Guid.NewGuid().ToString("N");
        var customerId = await _customerRepository.CreateCustomerAsync(customer);

        return customerId;
    }

    public Task UpdateCustomerNameAsync(string customerId, string firstName, string lastName)
    {
        return _customerRepository.UpdateCustomerNameAsync(customerId, firstName, lastName);
    }

    public Task UpdateCustomerEmailAddressAsync(string customerId, string emailAddress)
    {
        return _customerRepository.UpdateCustomerEmailAddressAsync(customerId, emailAddress);
    }

    public Task UpdateCustomerPhoneNumberAsync(string customerId, string phoneNumber)
    {
        return _customerRepository.UpdateCustomerPhoneNumberAsync(customerId, phoneNumber);
    }

    public async Task<OperationResult> UpdateCustomerCountryAsync(string customerId, string country)
    {
        var isUpdated = await _customerRepository.UpdateCustomerCountryAsync(customerId, country);
        if (isUpdated == false)
        {
            return OperationResult.Failure("Customer is not found", "404");
        }

        return OperationResult.Success();
    }

    public async Task<OperationResult> UpdateCustomerStatusAsync(string customerId, CustomerStatus status)
    {
        var isUpdated = await _customerRepository.UpdateCustomerStatusAsync(customerId, status);
        if (isUpdated == false)
        {
            return OperationResult.Failure("Customer is not found", "404");
        }

        return OperationResult.Success();
    }
}
