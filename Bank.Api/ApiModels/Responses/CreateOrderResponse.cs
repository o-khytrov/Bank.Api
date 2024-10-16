using System.Text.Json.Serialization;

namespace Bank.Api.ApiModels.Responses;

public record CreateOrderResponse
{
    [JsonPropertyName("request_id")]
    public string OrderId { get; set; }
}