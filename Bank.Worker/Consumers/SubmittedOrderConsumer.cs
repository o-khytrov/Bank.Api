using System.Text.Json;
using Bank.Common;
using Bank.Common.Messaging;
using Bank.Data.Repositories;
using MassTransit;

namespace Bank.Worker.Consumers;

internal class SubmitOrderConsumer(ILogger<SubmitOrderConsumer> logger, IOrderRepository orderRepository) : IConsumer<SubmitOrderMessage>
{
    public async Task Consume(ConsumeContext<SubmitOrderMessage> context)
    {
        var orderId = await orderRepository.InsertOrder(context.Message.Order);
        logger.LogInformation($"Order submitted {JsonSerializer.Serialize(context.Message)}");
        await context.RespondAsync(new OrderSubmitted { OrderId = orderId });
    }
}