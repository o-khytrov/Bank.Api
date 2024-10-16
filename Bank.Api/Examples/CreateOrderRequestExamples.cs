using Bank.Api.Models;
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