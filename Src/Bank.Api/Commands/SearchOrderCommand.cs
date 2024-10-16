using Bank.Common;
using MediatR;

namespace Bank.Api.Commands;

public record SearchOrderCommand(
    int? OrderId = null,
    string? ClientId = null,
    string? DepartmentAddress = null) : IRequest<SearchOrdersCommandResult>;

public record SearchOrdersCommandResult(IEnumerable<Order> Orders);