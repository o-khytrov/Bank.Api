using Bank.Api.Commands;
using Bank.Common.Messaging;
using MassTransit;
using MediatR;

namespace Bank.Api.Handlers;

public class SearchOrdersRequestHandler(IRequestClient<SearchOrderMessage> client) : IRequestHandler<SearchOrderCommand, SearchOrdersCommandResult>
{
    public async Task<SearchOrdersCommandResult> Handle(SearchOrderCommand request, CancellationToken cancellationToken)
    {
        var response = await client.GetResponse<SearchOrdersReply>(new SearchOrderMessage
        {
            DepartmentAddress = request.DepartmentAddress,
            OrderId = request.OrderId,
            ClientId = request.ClientId
        }, cancellationToken);

        return new SearchOrdersCommandResult(response.Message.Orders);
    }
}