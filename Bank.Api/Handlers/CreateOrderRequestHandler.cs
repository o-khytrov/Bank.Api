using Bank.Api.Models;
using MediatR;

namespace Bank.Api.Handlers;

public class CreateOrderRequestHandler : IRequestHandler<CreteOrderRequest, CreateOrderResponse>
{
    public Task<CreateOrderResponse> Handle(CreteOrderRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new CreateOrderResponse() { OrderId = Guid.NewGuid().ToString() });
    }
}