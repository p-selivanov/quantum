using System;

namespace Quantum.Account.Api.Models;

public class TransactionInfo
{
    public string Currency { get; set; }

    public decimal Amount { get; set; }

    public decimal Commission { get; set; }

    public DateTime Timestamp { get; set; }
}
