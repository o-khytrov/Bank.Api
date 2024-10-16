using Bank.Api.ApiModels;
using Bank.Api.ApiModels.Requests;
using Bank.Api.ApiModels.Responses;
using Bank.Api.ApiModels.Validation;
using Bank.Api.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController(
    IMediator mediator,
    CreateOrderRequestValidator createOrderRequestValidator,
    SearchOrderApiRequestValidator searchRequestValidator,
    IHttpContextAccessor httpContextAccessor
) : ControllerBase
{
    /// <summary>
    /// Create new order
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("create")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var validationResult = createOrderRequestValidator.Validate(request);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
        var ipAddress = GetClientIpAddress();
        var command = new CreateOrderCommand(request.ClientId, request.DepartmentAddress, request.Amount, request.Currency, ipAddress);

        var result = await mediator.Send(command, cancellationToken);
        return Ok(new CreateOrderResponse(result.OrderId));
    }

    /// <summary>
    /// Search order by request_id, or client_id and department_address
    /// </summary>
    /// <param name="searchRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("search")]
    public async Task<IActionResult> SearchOrder([FromBody] SearchOrderApiRequest searchRequest, CancellationToken cancellationToken)
    {
        var validationResult = searchRequestValidator.Validate(searchRequest);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
        var command = new SearchOrderCommand(searchRequest.OrderId, searchRequest.ClientId, searchRequest.DepartmentAddress);
        var result = await mediator.Send(command, cancellationToken);
        return Ok(new SearchOrdersResponse(result.Orders.Select(x => new OrderApiModel(x))));
    }

    private string? GetClientIpAddress()
    {
        var remoteIpAddress = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress;


        if (remoteIpAddress is { IsIPv4MappedToIPv6: true }) remoteIpAddress = remoteIpAddress.MapToIPv4();

        return remoteIpAddress?.ToString();
    }
}