namespace Bank.Common.Messaging;

public class SearchOrderMessage
{
    public string? DepartmentAddress { get; set; }
    public int? OrderId { get; set; }
    public string? ClientId { get; set; }
}