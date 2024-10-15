using System.Data;
using Bank.Common;
using Dapper;
using Npgsql;

namespace Bank.Data.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly string _connectionString;

    public OrderRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InsertOrder(Order order, CancellationToken cancellationToken = default)
    {
        await using var dbConnection = new NpgsqlConnection(_connectionString);
        var parameters = new
        {
            p_clientid = order.ClientId,
            p_address = order.Address,
            p_amount = order.Amount,
            p_currency = order.Currency,
            p_clientip = order.ClientId
        };

        await dbConnection.ExecuteAsync("InsertOrderProc", parameters, commandType: CommandType.StoredProcedure);
    }
}