using FluentValidation;

namespace Quantum.Account.Api.Dtos.Validators;

public class TransactionCreateRequestValidator : AbstractValidator<TransactionCreateRequest>
{
	public TransactionCreateRequestValidator()
	{
        RuleFor(x => x.Amount)
           .GreaterThan(0m);
    }
}
