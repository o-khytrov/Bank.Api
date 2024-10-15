namespace Bank.Common;

public class Order
{
    /// <summary>
    /// Id of the order 
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Unique identifier for the client placing the order 
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// Address of the client or the location related to the order
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// The amount of money involved in the order
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// The currency type used for the order (e.g., UAH, USD, EUR) 
    /// </summary>
    public Currency Currency { get; set; }

    /// <summary>
    /// IP address of the client making the request
    /// </summary>
    public string ClientIp { get; set; }
}