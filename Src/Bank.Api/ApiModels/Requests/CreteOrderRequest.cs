using System.Text.Json.Serialization;
using Bank.Common;

namespace Bank.Api.ApiModels.Requests;

public record CreteOrderRequest(
    [property: JsonPropertyName("client_id")]
    string ClientId,
    [property: JsonPropertyName("department_address")]
    string DepartmentAddress,
    [property: JsonPropertyName("amount")]
    decimal Amount,
    [property: JsonPropertyName("currency")]
    Currency Currency
);