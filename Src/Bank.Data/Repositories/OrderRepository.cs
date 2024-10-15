﻿using System.Data;
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

    public async Task<int> InsertOrder(Order order, CancellationToken cancellationToken = default)
    {
        await using var dbConnection = new NpgsqlConnection(_connectionString);
        var parameters = new DynamicParameters();

        // Add input parameters
        parameters.Add("p_clientid", order.ClientId);
        parameters.Add("p_address", order.Address);
        parameters.Add("p_amount", order.Amount);
        parameters.Add("p_currency", order.Currency);
        parameters.Add("p_clientip", order.ClientIp);
        parameters.Add("p_orderid", dbType: DbType.Int32, direction: ParameterDirection.Output);


        await dbConnection.ExecuteAsync("InsertOrderProc", parameters, commandType: CommandType.StoredProcedure);
        var orderId = parameters.Get<int>("p_orderid");
        return orderId;
    }

    public async Task<IEnumerable<Order>> SearchOrders(int? orderId = null, string? clientId = null, string? address = null, CancellationToken cancellationToken = default)
    {
        await using var dbConnection = new NpgsqlConnection(_connectionString);

        // Create DynamicParameters object
        var parameters = new DynamicParameters();

        // Add parameters if they are provided
        if (orderId.HasValue)
        {
            parameters.Add("p_orderid", orderId, DbType.Int32);
        }
        else
        {
            parameters.Add("p_orderid", null, DbType.Int32);
        }

        parameters.Add("p_clientid", clientId, DbType.String);
        parameters.Add("p_address", address, DbType.String);

        // Execute the stored procedure and return the results
        var orders = await dbConnection.QueryAsync<Order>("Select * FROM SearchOrdersProc(@p_orderid, @p_clientid, @p_address)", parameters, commandType: CommandType.Text);

        return orders;
    }
}