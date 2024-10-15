using Bank.Api.Models;
using Bank.Common;
using MassTransit;
using MediatR;

namespace Bank.Api.Handlers;

public class SearchOrdersRequestHandler(IRequestClient<SearchOrderRequest> client) : IRequestHandler<SearchOrderApiRequest, IEnumerable<Order>>
{
    public async Task<IEnumerable<Order>> Handle(SearchOrderApiRequest request, CancellationToken cancellationToken)
    {
        var response = await client.GetResponse<OrderSearchResult>(new SearchOrderRequest
        {
            DepartmentAddress = request.DepartmentAddress,
            OrderId = request.OrderId,
            ClientId = request.ClientId,
        }, cancellationToken);

        return response.Message.Orders;
    }
}