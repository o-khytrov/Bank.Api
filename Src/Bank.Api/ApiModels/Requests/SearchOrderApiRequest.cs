using System.Text.Json.Serialization;

namespace Bank.Api.ApiModels.Requests;

/// <summary>
/// Request for searching orders
/// </summary>
/// <param name="ClientId"></param>
/// <param name="DepartmentAddress"></param>
/// <param name="OrderId"></param>
public record SearchOrderApiRequest(
    [property: JsonPropertyName("client_id")]
    string? ClientId = null,
    [property: JsonPropertyName("department_address")]
    string? DepartmentAddress = null,
    [property: JsonPropertyName("request_id")]
    int? OrderId = null
);