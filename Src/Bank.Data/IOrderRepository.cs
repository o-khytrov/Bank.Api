namespace Bank.Data;

public interface IOrderRepository
{
    Task InsertOrder(string clientId, string address, decimal amount, string currency, string clientIp, CancellationToken cancellationToken = default);
}