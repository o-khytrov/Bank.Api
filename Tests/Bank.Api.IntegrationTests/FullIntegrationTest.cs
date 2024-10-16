using System.Net;
using Bank.Api.ApiModels.Requests;
using Bank.Api.ApiModels.Responses;
using Bank.Common;
using Bank.Worker;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace Bank.Api.IntegrationTests;

[TestFixture]
public class FullIntegrationTest
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;
    private PostgreSqlContainer _postgresContainer;
    private RabbitMqContainer _rabbitMqContainer;
    private IHost _workerHost;


    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _postgresContainer = new PostgreSqlBuilder()
            .WithDatabase("bankdb")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithCleanUp(true)
            .WithImage("postgres:latest")
            .Build();


        _rabbitMqContainer = new RabbitMqBuilder()
            .WithUsername("user")
            .WithPassword("password")
            .WithCleanUp(true)
            .WithImage("rabbitmq:management")
            .Build();

        await _postgresContainer.StartAsync();
        await _rabbitMqContainer.StartAsync();

        var rabbitConnection = _rabbitMqContainer.GetConnectionString();
        Environment.SetEnvironmentVariable("RabbitMQ__Host", _rabbitMqContainer.Hostname);
        Environment.SetEnvironmentVariable("RabbitMQ__Port", _rabbitMqContainer.GetMappedPublicPort(5672).ToString());
        Environment.SetEnvironmentVariable("ConnectionStrings__BankDb", _postgresContainer.GetConnectionString());
        _workerHost = Bank.Worker.Program.CreateWorkerHost([]);
        _workerHost.Services.MigrateDb();
        await _workerHost.StartAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _postgresContainer.StopAsync();
        await _rabbitMqContainer.StopAsync();
        await _postgresContainer.DisposeAsync();
        await _rabbitMqContainer.DisposeAsync();
        await _workerHost.StopAsync();
        _workerHost.Dispose();
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
    }

    [Test]
    public async Task FullIntegrationTest_HappyPath()
    {
        const string departmentAddress = "Kharkivs'ka St, 32";
        const string clientId = "14360570";
        // Arrange & Act
        var createOrderHttpResponse = await _client.PostAsync("/order/create", new CreteOrderRequest(
            ClientId: clientId,
            DepartmentAddress: departmentAddress,
            Amount: 500,
            Currency: Currency.UAH
        ).ToJsonContent());

        // Assert
        createOrderHttpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var createOrderHttpResponseContent = await createOrderHttpResponse.Content.ReadAsStringAsync();
        var createOrderResponse = createOrderHttpResponseContent.Deserialize<CreateOrderResponse>();
        createOrderResponse.Should().NotBeNull();
        createOrderResponse?.OrderId.Should().Be(1);

        var searchOrdersHttpResponse = await _client.PostAsync("/order/search", new SearchOrderApiRequest(
            ClientId: clientId, DepartmentAddress: departmentAddress
        ).ToJsonContent());


        var searchOrdersHttpResponseContent = await searchOrdersHttpResponse.Content.ReadAsStringAsync();
        var searchOrdersResponse = searchOrdersHttpResponseContent.Deserialize<SearchOrdersResponse>();
        searchOrdersResponse.Should().NotBeNull();
        searchOrdersResponse?.Orders.Should().NotBeEmpty();
    }
}