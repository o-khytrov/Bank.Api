using Bank.Common;
using MediatR;

namespace Bank.Api.Commands;

/// <inheritdoc />
public record CreateOrderCommand(
    string ClientId,
    string DepartmentAddress,
    decimal Amount,
    Currency Currency,
    string ClientIpAddress) : IRequest<CreateOrderCommandResult>;

/// <summary>
/// CreateOrderCommandResult
/// </summary>
/// <param name="OrderId"></param>
public record CreateOrderCommandResult(
    int OrderId
);