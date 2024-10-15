namespace Bank.Common;

public class SearchOrderRequest
{
    public string? DepartmentAddress { get; set; }
    public int? OrderId { get; set; }
    public string? ClientId { get; set; }
}