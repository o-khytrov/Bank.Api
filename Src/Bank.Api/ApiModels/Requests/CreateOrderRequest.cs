﻿using System.Text.Json.Serialization;
using Bank.Common;

namespace Bank.Api.ApiModels.Requests;

/// <summary>
/// Request for submitting new order
/// </summary>
/// <param name="ClientId"></param>
/// <param name="DepartmentAddress"></param>
/// <param name="Amount"></param>
/// <param name="Currency"></param>
public record CreateOrderRequest(
    [property: JsonPropertyName("client_id")]
    string ClientId,
    [property: JsonPropertyName("department_address")]
    string DepartmentAddress,
    [property: JsonPropertyName("amount")]
    decimal Amount,
    [property: JsonPropertyName("currency")]
    Currency Currency
);