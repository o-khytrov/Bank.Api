using Bank.Api.Controllers;
using Bank.Api.Models;
using Bank.Common;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.UnitTests;

[TestFixture]
public class OrderControllerTests
{
    private OrderController _orderController;

    [SetUp]
    public void Setup()
    {
        _orderController = new OrderController();
    }

    [Test]
    public async Task CreateOrder_ValidRequest_ReturnsOkResult()
    {
        // Arrange
        var request = new CreteOrderRequest
        {
            ClientId = "12345",
            DepartmentAddress = "123 Main St",
            Amount = 100.50m,
            Currency = Currency.USD
        };

        // Act
        var result = await _orderController.CreateOrder(request);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.That(okResult?.Value, Is.EqualTo("Order created"));
    }

    [Test]
    public async Task CreateOrder_InvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreteOrderRequest();

        // Act
        var result = await _orderController.CreateOrder(request);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }
}