using System.Data;
using Bank.Common;
using Dapper;
using Npgsql;

namespace Bank.Data.Repositories;

public class NpgsqlOrderRepository(string connectionString) : IOrderRepository
{
    public async Task<int> InsertOrder(Order order, CancellationToken cancellationToken = default)
    {
        await using var dbConnection = new NpgsqlConnection(connectionString);
        var parameters = new DynamicParameters();

        parameters.Add("p_client_id", order.ClientId);
        parameters.Add("p_department_address", order.DepartmentAddress);
        parameters.Add("p_amount", order.Amount);
        parameters.Add("p_currency", order.Currency);
        parameters.Add("p_client_ip", order.ClientIp);
        parameters.Add("p_order_id", dbType: DbType.Int32, direction: ParameterDirection.Output);


        await dbConnection.ExecuteAsync("sp_order_insert", parameters, commandType: CommandType.StoredProcedure);
        var orderId = parameters.Get<int>("p_order_id");
        return orderId;
    }

    public async Task<IEnumerable<Order>> SearchOrders(int? orderId = null, string? clientId = null, string? address = null, CancellationToken cancellationToken = default)
    {
        await using var dbConnection = new NpgsqlConnection(connectionString);

        var parameters = new DynamicParameters();

        parameters.Add("p_order_id", orderId, DbType.Int32);
        parameters.Add("p_client_id", clientId, DbType.String);
        parameters.Add("p_department_address", address, DbType.String);

        var orders = await dbConnection.QueryAsync<Order>("Select * FROM fn_orders_search(@p_order_id, @p_client_id, @p_department_address)", parameters, commandType: CommandType.Text);

        return orders;
    }
}