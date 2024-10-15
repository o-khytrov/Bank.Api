using Bank.Common;

namespace Bank.Data.Repositories;

public interface IOrderRepository
{
    Task<int> InsertOrder(Order order, CancellationToken cancellationToken = default);

    Task<IEnumerable<Order>> SearchOrders(int? orderId = null, string? clientId = null, string? address = null, CancellationToken cancellationToken = default);
}