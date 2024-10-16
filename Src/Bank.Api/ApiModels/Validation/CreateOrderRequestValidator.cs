using Bank.Api.ApiModels.Requests;
using FluentValidation;

namespace Bank.Api.ApiModels.Validation;

/// <inheritdoc />
public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    /// <inheritdoc />
    public CreateOrderRequestValidator()
    {
        RuleFor(r => r.Amount)
            .GreaterThanOrEqualTo(100)
            .LessThanOrEqualTo(100000);

        RuleFor(x => x.ClientId).NotEmpty().MaximumLength(128);
        RuleFor(x => x.Currency).NotEmpty().IsInEnum();
        RuleFor(x => x.DepartmentAddress).NotEmpty().MaximumLength(256);
    }
}