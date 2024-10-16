using Bank.Common;
using MediatR;

namespace Bank.Api.Commands;

/// <inheritdoc />
public record SearchOrderCommand(
    int? OrderId = null,
    string? ClientId = null,
    string? DepartmentAddress = null) : IRequest<SearchOrdersCommandResult>;

/// <summary>
/// SearchOrdersCommandResult
/// </summary>
/// <param name="Orders"></param>
public record SearchOrdersCommandResult(IEnumerable<Order> Orders);