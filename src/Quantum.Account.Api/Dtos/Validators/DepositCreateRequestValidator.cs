using System;
using System.Linq;
using FluentValidation;
using Microsoft.Extensions.Options;
using Quantum.Account.Api.Configuration;

namespace Quantum.Account.Api.Dtos.Validators;

public class DepositCreateRequestValidator : AbstractValidator<DepositCreateRequest>
{
	public DepositCreateRequestValidator(IOptions<CurrencyOptions> currrencyOptions)
	{
        var currencies = currrencyOptions.Value.Currencies;

        RuleFor(x => x.Currency)
            .NotEmpty()
            .Must(x => currencies.Contains(x, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Currency must be in: " + string.Join(", ", currencies));

        RuleFor(x => x.Amount)
           .GreaterThan(0m);
    }
}
