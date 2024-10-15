using Bank.Api.Models;
using Bank.Common;
using MassTransit;
using MediatR;

namespace Bank.Api.Handlers;

public class CreateOrderRequestHandler(IBus bus) : IRequestHandler<CreteOrderRequest, CreateOrderResponse>
{
    public Task<CreateOrderResponse> Handle(CreteOrderRequest request, CancellationToken cancellationToken)
    {
        bus.Publish(new Order
        {
            ClientId = request.ClientId,
            Address = request.DepartmentAddress,
            Amount = request.Amount,
            Currency = request.Currency,
            ClientIp = request.ClientId
        }, cancellationToken);
        return Task.FromResult(new CreateOrderResponse { OrderId = Guid.NewGuid().ToString() });
    }
}