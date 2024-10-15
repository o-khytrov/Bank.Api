using System.Text.Json;
using Bank.Common;
using Bank.Data.Repositories;
using MassTransit;

namespace Bank.Worker.Consumers;

class SearchOrderConsumer(ILogger<SearchOrderConsumer> logger, IOrderRepository orderRepository) : IConsumer<SearchOrderRequest>
{
    public async Task Consume(ConsumeContext<SearchOrderRequest> context)
    {
        var orders = await orderRepository.SearchOrders(context.Message.OrderId, context.Message.ClientId, context.Message.DepartmentAddress);
        logger.LogInformation($"Order submitted {JsonSerializer.Serialize(context.Message)}");
        await context.RespondAsync(new OrderSearchResult { Orders = orders.ToList() });
    }
}