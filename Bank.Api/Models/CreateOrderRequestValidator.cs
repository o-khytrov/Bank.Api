using FluentValidation;

namespace Bank.Api.Models;

public class CreateOrderRequestValidator : AbstractValidator<CreteOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(r => r.Amount)
            .GreaterThanOrEqualTo(100)
            .LessThanOrEqualTo(100000);
    }
}