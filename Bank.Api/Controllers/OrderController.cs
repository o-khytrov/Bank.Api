using Bank.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController(IMediator mediator, CreateOrderRequestValidator validator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreteOrderRequest request, CancellationToken cancellationToken)
    {
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        CreateOrderResponse result = await mediator.Send(request, cancellationToken);
        return Ok(result);
    }
}