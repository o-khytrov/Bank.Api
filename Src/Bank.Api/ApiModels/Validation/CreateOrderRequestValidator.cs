using Bank.Api.ApiModels.Requests;
using FluentValidation;

namespace Bank.Api.ApiModels.Validation;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(r => r.Amount)
            .GreaterThanOrEqualTo(100)
            .LessThanOrEqualTo(100000);

        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.Currency).NotEmpty().IsInEnum();
        RuleFor(x => x.DepartmentAddress).NotEmpty();
    }
}