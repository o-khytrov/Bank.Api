using System.Text.Json.Serialization;

namespace Bank.Api.ApiModels.Responses;

/// <summary>
/// Response for found orders
/// </summary>
/// <param name="Orders"></param>
public record SearchOrdersResponse(
    [property: JsonPropertyName("orders")]
    IEnumerable<OrderApiModel> Orders);