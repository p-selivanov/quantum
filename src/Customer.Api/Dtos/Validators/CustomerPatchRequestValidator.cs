﻿using FluentValidation;

namespace Quantum.Customer.Api.Dtos.Validators;

public class CustomerPatchRequestValidator : AbstractValidator<CustomerPatchRequest>
{
    public CustomerPatchRequestValidator()
    {
        RuleFor(x => x.EmailAddress.Value)
            .EmailAddress()
            .MaximumLength(100)
            .When(x => x.EmailAddress.IsSpecified);

        RuleFor(x => x.FirstName.Value)
            .MaximumLength(50)
            .When(x => x.FirstName.IsSpecified);

        RuleFor(x => x.LastName.Value)
            .MaximumLength(50)
            .When(x => x.LastName.IsSpecified);

        RuleFor(x => x.PhoneNumber.Value)
           .MaximumLength(20)
           .When(x => x.PhoneNumber.IsSpecified);

        RuleFor(x => x.Country.Value)
           .MaximumLength(20)
           .When(x => x.Country.IsSpecified);

        RuleFor(x => x.Status.Value)
           .IsInEnum()
           .When(x => x.Status.IsSpecified);
    }
}
