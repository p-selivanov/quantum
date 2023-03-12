using FluentValidation;

namespace Quantum.Customer.Api.Dtos.Validators;

public class CustomerCreateRequestValidator : AbstractValidator<CustomerCreateRequest>
{
    public CustomerCreateRequestValidator()
    {
        RuleFor(x => x.EmailAddress)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(100);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.PhoneNumber)
           .MaximumLength(20);

        RuleFor(x => x.Country)
           .MaximumLength(20);
    }
}
