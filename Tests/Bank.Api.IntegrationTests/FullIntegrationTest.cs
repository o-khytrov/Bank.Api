﻿using System.Net;
using Bank.Api.ApiModels.Requests;
using Bank.Api.ApiModels.Responses;
using Bank.Common;
using Bank.Data;
using Bank.Worker;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Testcontainers.MsSql;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace Bank.Api.IntegrationTests;

[TestFixture]
public class FullIntegrationTest
{
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        InitPostgreSqlContainer();
        InitMsSqlContainer();
        InitRabbitMqContainer();
        await StartContainers();
        SetEnvironmentVariables();
    }


    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await StopAndDisposeContainers();
    }


    [SetUp]
    public void SetUp()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
        _workerHost.StopAsync().GetAwaiter().GetResult();
        _workerHost.Dispose();
    }

    private void StartWorker()
    {
        _workerHost = Worker.Program.CreateWorkerHost([]);
        _workerHost.Services.MigrateDb();
        _workerHost.Start();
    }

    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;
    private PostgreSqlContainer _postgresContainer;
    private RabbitMqContainer _rabbitMqContainer;
    private MsSqlContainer _msSqlContainer;
    private IHost _workerHost;

    [TestCase(DbProvider.PostgreSql)]
    [TestCase(DbProvider.MsSql)]
    public async Task FullIntegrationTest_HappyPath(DbProvider dbProvider)
    {
        // Arrange
        Environment.SetEnvironmentVariable(nameof(dbProvider), dbProvider.ToString());
        StartWorker();
        const string departmentAddress = "Kharkivs'ka St, 32";
        const string clientId = "14360570";

        // Act
        var createOrderHttpResponse = await _client.PostAsync("/order/create", new CreateOrderRequest(
            clientId,
            departmentAddress,
            500,
            Currency.UAH
        ).ToJsonContent());

        // Assert
        createOrderHttpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var createOrderHttpResponseContent = await createOrderHttpResponse.Content.ReadAsStringAsync();
        var createOrderResponse = createOrderHttpResponseContent.Deserialize<CreateOrderResponse>();
        createOrderResponse.Should().NotBeNull();
        createOrderResponse?.OrderId.Should().Be(1);

        var searchOrdersHttpResponse = await _client.PostAsync("/order/search", new SearchOrderApiRequest(
            clientId, departmentAddress
        ).ToJsonContent());


        var searchOrdersHttpResponseContent = await searchOrdersHttpResponse.Content.ReadAsStringAsync();
        var searchOrdersResponse = searchOrdersHttpResponseContent.Deserialize<SearchOrdersResponse>();
        searchOrdersResponse.Should().NotBeNull();
        searchOrdersResponse?.Orders.Should().NotBeEmpty();
        var order = searchOrdersResponse?.Orders.First() ?? throw new Exception();
        order.OrderId.Should().Be(1);
        order.DepartmentAddress.Should().Be(departmentAddress);
        order.Amount.Should().Be(500);
        order.Currency.Should().Be(Currency.UAH);
        order.ClientId.Should().Be(clientId);
    }

    private async Task StartContainers()
    {
        await _postgresContainer.StartAsync();
        await _rabbitMqContainer.StartAsync();
        await _msSqlContainer.StartAsync();
    }

    private void InitMsSqlContainer()
    {
        _msSqlContainer = new MsSqlBuilder()
            .WithPassword("yourStrong(!)Password")
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .Build();
    }

    private void InitRabbitMqContainer()
    {
        _rabbitMqContainer = new RabbitMqBuilder()
            .WithUsername("user")
            .WithPassword("password")
            .WithCleanUp(true)
            .WithImage("rabbitmq:management")
            .Build();
    }

    private void InitPostgreSqlContainer()
    {
        _postgresContainer = new PostgreSqlBuilder()
            .WithDatabase("bankdb")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithCleanUp(true)
            .WithImage("postgres:latest")
            .Build();
    }

    private async Task StopAndDisposeContainers()
    {
        await _postgresContainer.StopAsync();
        await _rabbitMqContainer.StopAsync();
        await _msSqlContainer.StopAsync();
        await _postgresContainer.DisposeAsync();
        await _rabbitMqContainer.DisposeAsync();
        await _msSqlContainer.DisposeAsync();
    }

    private void SetEnvironmentVariables()
    {
        Environment.SetEnvironmentVariable("RabbitMQ__Host", _rabbitMqContainer.Hostname);
        Environment.SetEnvironmentVariable("RabbitMQ__Port", _rabbitMqContainer.GetMappedPublicPort(5672).ToString());
        Environment.SetEnvironmentVariable("ConnectionStrings__BankDbPostgresql", _postgresContainer.GetConnectionString());
        Environment.SetEnvironmentVariable("ConnectionStrings__BankDbMssql", _msSqlContainer.GetConnectionString());
    }
}