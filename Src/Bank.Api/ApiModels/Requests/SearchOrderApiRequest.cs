using System.Text.Json.Serialization;

namespace Bank.Api.ApiModels.Requests;

public record SearchOrderApiRequest(
    [property: JsonPropertyName("client_id")]
    string? ClientId = null,
    [property: JsonPropertyName("department_address")]
    string? DepartmentAddress = null,
    [property: JsonPropertyName("request_id")]
    int? OrderId = null
);