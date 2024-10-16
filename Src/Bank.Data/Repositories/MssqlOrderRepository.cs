using Bank.Common;

namespace Bank.Data.Repositories;

public class MssqlOrderRepository(string connectionString) : IOrderRepository
{
    public Task<int> InsertOrder(Order order, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Order>> SearchOrders(int? orderId = null, string? clientId = null, string? address = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}