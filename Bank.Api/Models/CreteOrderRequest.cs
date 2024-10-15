using System.Text.Json.Serialization;
using Bank.Common;

namespace Bank.Api.Models;

public class CreteOrderRequest
{
    [JsonPropertyName("client_id")]
    public string ClientId { get; set; }

    [JsonPropertyName("department_address")]
    public string DepartmentAddress { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("currency")]
    public Currency Currency { get; set; }
}