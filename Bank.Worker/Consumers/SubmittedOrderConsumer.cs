using System.Text.Json;
using Bank.Common;
using Bank.Data.Repositories;
using MassTransit;

namespace Bank.Worker.Consumers;

class SubmitOrderConsumer(ILogger<SubmitOrderConsumer> logger, IOrderRepository orderRepository) : IConsumer<Order>
{
    public async Task Consume(ConsumeContext<Order> context)
    {
        var orderId = await orderRepository.InsertOrder(context.Message);
        logger.LogInformation($"Order submitted {JsonSerializer.Serialize(context.Message)}");
        await context.RespondAsync(new OrderSubmitted { OrderId = orderId });
    }
}