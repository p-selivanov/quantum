using FluentValidation;

namespace Quantum.Customer.Api.Dtos.Validators;

public class CustomerPatchRequestValidator : AbstractValidator<CustomerPatchRequest>
{
    public CustomerPatchRequestValidator()
    {
        RuleFor(x => x.Type.Value)
            .IsInEnum()
            .When(x => x.Type.IsSpecified);

        RuleFor(x => x.AgentId.Value)
            .GreaterThan(0)
            .When(x => x.AgentId.IsSpecified && x.AgentId.Value.HasValue);
    }
}
