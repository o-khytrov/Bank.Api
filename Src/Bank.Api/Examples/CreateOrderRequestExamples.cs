using Bank.Api.ApiModels.Requests;
using Bank.Common;
using Swashbuckle.AspNetCore.Filters;

namespace Bank.Api.Examples;

public class CreateOrderRequestExamples : IMultipleExamplesProvider<CreateOrderRequest>
{
    public IEnumerable<SwaggerExample<CreateOrderRequest>> GetExamples()
    {
        yield return SwaggerExample.Create("Create new order request", new CreateOrderRequest
        (
            "14360570",
            "Kharkivs'ka St, 32",
            500,
            Currency.UAH
        ));
    }
}

public class SearchOrdersRequestExamples : IMultipleExamplesProvider<SearchOrderApiRequest>
{
    public IEnumerable<SwaggerExample<SearchOrderApiRequest>> GetExamples()
    {
        yield return SwaggerExample.Create("Search by OrderId", new SearchOrderApiRequest(OrderId: 1));
        yield return SwaggerExample.Create("Search by ClientId and DepartmentAddress", new SearchOrderApiRequest("14360570", "Kharkivs'ka St, 32"));
    }
}