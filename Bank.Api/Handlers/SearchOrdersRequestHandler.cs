using Bank.Api.Commands;
using Bank.Common;
using MassTransit;
using MediatR;

namespace Bank.Api.Handlers;

public class SearchOrdersRequestHandler(IRequestClient<SearchOrderRequest> client) : IRequestHandler<SearchOrderCommand, SearchOrdersCommandResult>
{
    public async Task<SearchOrdersCommandResult> Handle(SearchOrderCommand request, CancellationToken cancellationToken)
    {
        var response = await client.GetResponse<OrderSearchResult>(new SearchOrderRequest
        {
            DepartmentAddress = request.DepartmentAddress,
            OrderId = request.OrderId,
            ClientId = request.ClientId
        }, cancellationToken);

        return new SearchOrdersCommandResult(response.Message.Orders);
    }
}