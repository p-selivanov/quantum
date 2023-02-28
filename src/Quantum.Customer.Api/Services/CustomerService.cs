using System;
using System.Threading.Tasks;
using Quantum.Customer.Models;
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
        customer.Id = Guid.NewGuid().ToString();
        customer.CreationTimestamp = DateTime.UtcNow;
        customer.UpdateTimestamp = DateTime.UtcNow;

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
            return OperationResult.NotFound();
        }

        return OperationResult.Success();
    }
}
