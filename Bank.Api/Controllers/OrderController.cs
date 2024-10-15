using Bank.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreteOrderRequest request, CancellationToken cancellationToken)
    {
        CreateOrderResponse result = await mediator.Send(request, cancellationToken);
        return Ok(result);
    }
}