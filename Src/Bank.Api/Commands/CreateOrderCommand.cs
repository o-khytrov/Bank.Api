using Bank.Common;
using MediatR;

namespace Bank.Api.Commands;

public record CreateOrderCommand(
    string ClientId,
    string DepartmentAddress,
    decimal Amount,
    Currency Currency,
    string? ClientIpAddress = null) : IRequest<CreateOrderCommandResult>;

public record CreateOrderCommandResult(int OrderId);