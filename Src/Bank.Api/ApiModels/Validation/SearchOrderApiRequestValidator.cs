﻿using Bank.Api.ApiModels.Requests;
using FluentValidation;

namespace Bank.Api.ApiModels.Validation;

/// <inheritdoc />
public class SearchOrderApiRequestValidator : AbstractValidator<SearchOrderApiRequest>
{
    /// <inheritdoc />
    public SearchOrderApiRequestValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty()
            .When(x => string.IsNullOrEmpty(x.DepartmentAddress) && string.IsNullOrEmpty(x.ClientId));

        RuleFor(x => x.DepartmentAddress).NotEmpty()
            .When(x => x.OrderId is null);

        RuleFor(x => x.ClientId).NotEmpty()
            .When(x => x.OrderId is null);
    }
}