using Bank.Api.ApiModels.Requests;
using Bank.Common;
using Swashbuckle.AspNetCore.Filters;

namespace Bank.Api.Examples;

public class CreateOrderRequestExamples : IMultipleExamplesProvider<CreteOrderRequest>
{
    public IEnumerable<SwaggerExample<CreteOrderRequest>> GetExamples()
    {
        yield return SwaggerExample.Create("Create new order request", new CreteOrderRequest
        {
            ClientId = "14360570",
            DepartmentAddress = "Kharkivs'ka St, 32",
            Amount = 500,
            Currency = Currency.UAH
        });
    }
}

public class SearchOrdersRequestExamples : IMultipleExamplesProvider<SearchOrderApiRequest>
{
    public IEnumerable<SwaggerExample<SearchOrderApiRequest>> GetExamples()
    {
        yield return SwaggerExample.Create("Search by OrderId", new SearchOrderApiRequest { ClientId = "14360570" });
        yield return SwaggerExample.Create("Search by ClientId and DepartmentAddress", new SearchOrderApiRequest { ClientId = "14360570", DepartmentAddress = "Kharkivs'ka St, 32" });
    }
}