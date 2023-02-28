using FluentValidation;

namespace Quantum.Account.Api.Dtos.Validators;

public class AccountCreateRequestValidator : AbstractValidator<AccountCreateRequest>
{
	public AccountCreateRequestValidator()
	{
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.Currency)
            .NotEmpty()
            .MaximumLength(20);
    }
}
