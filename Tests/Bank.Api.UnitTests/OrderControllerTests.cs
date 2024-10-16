using Bank.Api.ApiModels.Requests;
using Bank.Api.ApiModels.Responses;
using Bank.Api.ApiModels.Validation;
using Bank.Api.Commands;
using Bank.Api.Controllers;
using Bank.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Bank.Api.UnitTests;

[TestFixture]
public class OrderControllerTests
{
    [SetUp]
    public void Setup()
    {
        _mediatorMock = new Mock<IMediator>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        var context = new DefaultHttpContext();

        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(context);


        _orderController = new OrderController(_mediatorMock.Object, new CreateOrderRequestValidator(), new SearchOrderApiRequestValidator(), _httpContextAccessorMock.Object);
    }

    private OrderController _orderController;
    private Mock<IMediator> _mediatorMock;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock;

    [Test]
    public async Task CreateOrder_ValidRequest_ReturnsOkResult()
    {
        // Arrange
        var request = new CreateOrderRequest
        (
            "12345",
            "123 Main St",
            100.50m,
            Currency.USD
        );

        var expectedResponse = new CreateOrderResponse(42);

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateOrderCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CreateOrderCommandResult(42));

        // Act
        var result = await _orderController.CreateOrder(request, CancellationToken.None);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.That(okResult?.Value, Is.EqualTo(expectedResponse));
        _mediatorMock.Verify(m => m.Send(It.IsAny<CreateOrderCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task SearchOrder_ValidRequest_ReturnsOkResult()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<SearchOrderCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SearchOrdersCommandResult(new List<Order>()));

        // Act
        var result = await _orderController.SearchOrder(new SearchOrderApiRequest { OrderId = 1 }, CancellationToken.None);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);

        _mediatorMock.Verify(m => m.Send(It.IsAny<SearchOrderCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}