using System.Text.Json.Serialization;

namespace Bank.Api.ApiModels.Responses;

public record SearchOrdersResponse(
    [property: JsonPropertyName("orders")]
    IEnumerable<OrderApiModel> Orders);