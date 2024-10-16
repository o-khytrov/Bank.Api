using Bank.Api.ApiModels.Responses;
using Bank.Api.Commands;
using Bank.Common;
using MassTransit;
using MediatR;

namespace Bank.Api.Handlers;

public class CreateOrderRequestHandler(IRequestClient<Order> client) : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
{
    public async Task<CreateOrderResponse> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            ClientId = command.ClientId,
            Address = command.DepartmentAddress,
            Amount = command.Amount,
            Currency = command.Currency,
            ClientIp = command.ClientId
        };
        var response = await client.GetResponse<OrderSubmitted>(order, cancellationToken);

        return new CreateOrderResponse { OrderId = response.Message.OrderId.ToString() };
    }
}