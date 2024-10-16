﻿using Bank.Api.ApiModels.Requests;
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
    SearchOrderApiRequestValidator searchRequestValidator
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreteOrderRequest request, CancellationToken cancellationToken)
    {
        var validationResult = createOrderRequestValidator.Validate(request);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
        var ipAddress = GetClientIpAddress();
        var command = new CreateOrderCommand(request.ClientId, request.DepartmentAddress, request.Amount, request.Currency, ipAddress);

        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    private string GetClientIpAddress()
    {
        var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;


        if (remoteIpAddress != null && remoteIpAddress.IsIPv4MappedToIPv6) remoteIpAddress = remoteIpAddress.MapToIPv4();

        return remoteIpAddress.ToString();
    }

    [HttpGet]
    public async Task<IActionResult> SearchOrder([FromQuery] SearchOrderApiRequest searchRequest, CancellationToken cancellationToken)
    {
        var validationResult = searchRequestValidator.Validate(searchRequest);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
        var command = new SearchOrderCommand(searchRequest.OrderId, searchRequest.ClientId, searchRequest.DepartmentAddress);
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}