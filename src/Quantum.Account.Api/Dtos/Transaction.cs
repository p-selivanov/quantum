using System;

namespace Quantum.Account.Api.Dtos;

public class Transaction
{
    public string Currency { get; set; }

    public decimal Amount { get; set; }

    public DateTime Timestamp { get; set; }
}
