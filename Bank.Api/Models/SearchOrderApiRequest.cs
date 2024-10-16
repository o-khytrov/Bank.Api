using System.Text.Json.Serialization;
using Bank.Common;
using MediatR;

namespace Bank.Api.Models;

public record SearchOrderApiRequest : IRequest<IEnumerable<Order>>
{
    [JsonPropertyName("client_id")]
    public string? ClientId { get; set; }

    [JsonPropertyName("department_address")]
    public string? DepartmentAddress { get; set; }

    [JsonPropertyName("request_id:")]
    public int? OrderId { get; set; }
}