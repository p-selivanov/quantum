﻿using System;
using Quantum.Customer.Models;

namespace Quantum.Customer.Api.Dtos;

public class Customer
{
    public string Id { get; set; }

    public string EmailAddress { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string PhoneNumber { get; set; }

    public string Country { get; set; }

    public CustomerStatus Status { get; set; }

    public DateTime CreationTimestamp { get; set; }

    public DateTime UpdateTimestamp { get; set; }
}
