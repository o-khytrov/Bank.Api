using Bank.Api.Models;
using Bank.Common;
using MassTransit;
using MediatR;

namespace Bank.Api.Handlers;

public class CreateOrderRequestHandler(IRequestClient<Order> client) : IRequestHandler<CreteOrderRequest, CreateOrderResponse>
{
    public async Task<CreateOrderResponse> Handle(CreteOrderRequest request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            ClientId = request.ClientId,
            Address = request.DepartmentAddress,
            Amount = request.Amount,
            Currency = request.Currency,
            ClientIp = request.ClientId
        };
        var response = await client.GetResponse<OrderSubmitted>(order, cancellationToken);

        return new CreateOrderResponse { OrderId = response.Message.OrderId.ToString() };
    }
}