using System.Text.Json.Serialization;
using Bank.Common;

namespace Bank.Api.ApiModels.Responses;

public record SearchOrdersResponse(
    [property: JsonPropertyName("orders")]
    IEnumerable<Order> Orders);