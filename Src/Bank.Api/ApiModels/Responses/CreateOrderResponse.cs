using System.Text.Json.Serialization;

namespace Bank.Api.ApiModels.Responses;

/// <summary>
/// Response for successfully created order
/// </summary>
/// <param name="OrderId"></param>
public record CreateOrderResponse(
    [property: JsonPropertyName("request_id")]
    int OrderId
);