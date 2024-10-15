using Bank.Common;

namespace Bank.Data.Repositories;

public interface IOrderRepository
{
    Task InsertOrder(Order order, CancellationToken cancellationToken = default);
}