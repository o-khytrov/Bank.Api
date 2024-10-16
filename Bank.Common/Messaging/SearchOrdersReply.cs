namespace Bank.Common.Messaging;

public class SearchOrdersReply
{
    public List<Order> Orders { get; set; } = new();
}