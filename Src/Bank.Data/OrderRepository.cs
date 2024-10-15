using System.Data;
using Dapper;
using Npgsql;

namespace Bank.Data;

public class OrderRepository : IOrderRepository
{
    private readonly string _connectionString;

    public OrderRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task InsertOrder(string clientId, string address, decimal amount, string currency, string clientIp, CancellationToken cancellationToken = default)
    {
        await using var dbConnection = new NpgsqlConnection(_connectionString);
        var parameters = new
        {
            p_clientid = clientId,
            p_address = address,
            p_amount = amount,
            p_currency = currency,
            p_clientip = clientIp
        };

        await dbConnection.ExecuteAsync("InsertOrderProc", parameters, commandType: CommandType.StoredProcedure);
    }
}