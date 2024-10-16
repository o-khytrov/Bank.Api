using System.Text.Json;
using Bank.Common.Messaging;
using Bank.Data.Repositories;
using MassTransit;

namespace Bank.Worker.Consumers;

internal class SearchOrderConsumer(ILogger<SearchOrderConsumer> logger, IOrderRepository orderRepository) : IConsumer<SearchOrderMessage>
{
    public async Task Consume(ConsumeContext<SearchOrderMessage> context)
    {
        var orders = await orderRepository.SearchOrders(context.Message.OrderId, context.Message.ClientId, context.Message.DepartmentAddress);
        logger.LogInformation($"Order submitted {JsonSerializer.Serialize(context.Message)}");
        await context.RespondAsync(new SearchOrdersReply { Orders = orders.ToList() });
    }
}