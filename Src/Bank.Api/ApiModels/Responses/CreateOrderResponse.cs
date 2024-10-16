using System.Text.Json.Serialization;

namespace Bank.Api.ApiModels.Responses;

public record CreateOrderResponse(
    [property: JsonPropertyName("request_id")]
    int OrderId
);