using Bank.Api.ApiModels.Requests;
using Bank.Api.ApiModels.Responses;
using Bank.Api.ApiModels.Validation;
using Bank.Api.Controllers;
using Bank.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Bank.Api.UnitTests;

[TestFixture]
public class OrderControllerTests
{
    private OrderController _orderController;
    private Mock<IMediator> _mediatorMock;


    [SetUp]
    public void Setup()
    {
        _mediatorMock = new Mock<IMediator>();

        _orderController = new OrderController(_mediatorMock.Object, new CreateOrderRequestValidator(), new SearchOrderApiRequestValidator());
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

        var expectedResponse = new CreateOrderResponse()
        {
            OrderId = Guid.NewGuid().ToString()
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreteOrderRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _orderController.CreateOrder(request, CancellationToken.None);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.That(okResult?.Value, Is.EqualTo(expectedResponse));
        _mediatorMock.Verify(m => m.Send(It.IsAny<CreteOrderRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}