using System.Data;
using Bank.Common;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Bank.Data.Repositories;

public class MssqlOrderRepository(string connectionString) : IOrderRepository
{
    public async Task<int> InsertOrder(Order order, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(connectionString);
        var parameters = new DynamicParameters();
        parameters.Add("@clientId", order.ClientId);
        parameters.Add("@departmentaddress", order.DepartmentAddress);
        parameters.Add("@amount", order.Amount);
        parameters.Add("@currency", order.Currency);
        parameters.Add("@clientIp", order.ClientIp);
        parameters.Add("@orderId", dbType: DbType.Int32, direction: ParameterDirection.Output);
        await connection.ExecuteAsync("sp_order_insert", parameters, commandType: CommandType.StoredProcedure);
        var orderId = parameters.Get<int>("@orderId");
        return orderId;
    }

    public async Task<IEnumerable<Order>> SearchOrders(int? orderId = null, string? clientId = null, string? address = null, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(connectionString);

        var parameters = new DynamicParameters();

        parameters.Add("@orderId", orderId, DbType.Int32);
        parameters.Add("@clientId", clientId, DbType.String);
        parameters.Add("@departmentAddress", address, DbType.String);

        var orders = await connection.QueryAsync<Order>("sp_orders_search", parameters, commandType: CommandType.StoredProcedure);

        return orders;
    }
}