using Bank.Api.ApiModels;
using Bank.Api.ApiModels.Requests;
using Bank.Api.ApiModels.Responses;
using Bank.Api.ApiModels.Validation;
using Bank.Api.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers;

/// <summary>
///  APIs to create orders and search orders in the bank system
/// </summary>
/// <param name="mediator"></param>
/// <param name="createOrderRequestValidator"></param>
/// <param name="searchRequestValidator"></param>
/// <param name="httpContextAccessor"></param>
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
    /// <remarks>
    /// This endpoint allows clients to create a new order by providing necessary order details, such as the client ID, department address, order amount, and currency.
    /// Additionally, the server captures the client’s IP address for auditing and validation purposes.
    /// </remarks>
    /// <response code="200">When the request has been successful.</response>
    /// <response code="400">When there was an incorrect user input.</response>
    /// <response code="500">When there was an unexpected problem while processing the request</response>
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
    /// <remarks>
    /// This endpoint allows clients to search for orders based on either the Order ID or a combination of Client ID and Department Address. 
    /// </remarks>
    /// <response code="200">When the request has been successful.</response>
    /// <response code="400">When there was an incorrect user input.</response>
    /// <response code="500">When there was an unexpected problem while processing the request</response>
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

    private string GetClientIpAddress()
    {
        var remoteIpAddress = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress;


        if (remoteIpAddress is { IsIPv4MappedToIPv6: true }) remoteIpAddress = remoteIpAddress.MapToIPv4();

        return remoteIpAddress?.ToString() ?? "unknown";
    }
}