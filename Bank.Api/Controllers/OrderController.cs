using Bank.Api.Models;
using Bank.Api.Models.Validation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController(
    IMediator mediator,
    CreateOrderRequestValidator createOrderRequestValidator,
    SearchOrderApiRequestValidator searchRequestValidator
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreteOrderRequest request, CancellationToken cancellationToken)
    {
        var validationResult = createOrderRequestValidator.Validate(request);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var result = await mediator.Send(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> SearchOrder([FromQuery] SearchOrderApiRequest searchRequest, CancellationToken cancellationToken)
    {
        var validationResult = searchRequestValidator.Validate(searchRequest);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        var result = await mediator.Send(searchRequest, cancellationToken);
        return Ok(result);
    }
}