using Bank.Api.Commands;
using Bank.Common;
using Bank.Common.Messaging;
using MassTransit;
using MediatR;

namespace Bank.Api.Handlers;

public class CreateOrderRequestHandler(IRequestClient<SubmitOrderMessage> client) : IRequestHandler<CreateOrderCommand, CreateOrderCommandResult>
{
    public async Task<CreateOrderCommandResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            ClientId = command.ClientId,
            DepartmentAddress = command.DepartmentAddress,
            Amount = command.Amount,
            Currency = command.Currency,
            ClientIp = command.ClientId
        };
        var response = await client.GetResponse<SubmitOrderReply>(new SubmitOrderMessage(order), cancellationToken);

        return new CreateOrderCommandResult(response.Message.OrderId);
    }
}