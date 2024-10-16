using System.Text.Json.Serialization;
using Bank.Common;

namespace Bank.Api.ApiModels;

/// <summary>
/// Represent the order 
/// </summary>
public class OrderApiModel
{
    /// <summary>
    /// Default constructor (For testing)
    /// </summary>
    public OrderApiModel()
    {
    }

    /// <summary>
    /// Creates OrderApiModel from Order 
    /// </summary>
    /// <param name="order"></param>
    public OrderApiModel(Order order)
    {
        OrderId = order.OrderId;
        ClientId = order.ClientId;
        DepartmentAddress = order.DepartmentAddress;
        Amount = order.Amount;
        Currency = order.Currency;
    }

    /// <summary>
    ///     Id of the order
    /// </summary>
    [JsonPropertyName("order_id")]
    public int OrderId { get; set; }

    /// <summary>
    ///     Unique identifier for the client placing the order
    /// </summary>
    [JsonPropertyName("client_id")]
    public string ClientId { get; set; } = null!;

    /// <summary>
    ///     Address of the department
    /// </summary>

    [JsonPropertyName("department_address")]
    public string DepartmentAddress { get; set; } = null!;

    /// <summary>
    ///     The amount of money involved in the order
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    /// <summary>
    ///     The currency type used for the order (e.g., UAH, USD, EUR)
    /// </summary>
    [JsonPropertyName("currency")]
    public Currency Currency { get; set; }
}