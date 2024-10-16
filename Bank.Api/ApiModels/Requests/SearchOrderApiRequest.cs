using System.Text.Json.Serialization;

namespace Bank.Api.ApiModels.Requests;

public record SearchOrderApiRequest
{
    [JsonPropertyName("client_id")]
    public string? ClientId { get; set; }

    [JsonPropertyName("department_address")]
    public string? DepartmentAddress { get; set; }

    [JsonPropertyName("request_id")]
    public int? OrderId { get; set; }
}