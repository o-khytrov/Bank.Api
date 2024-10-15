using System.Text.Json;
using Bank.Common;
using MassTransit;

namespace Bank.Worker;

class SubmitOrderConsumer(ILogger<SubmitOrderConsumer> logger) : IConsumer<Order>
{
    public Task Consume(ConsumeContext<Order> context)
    {
        logger.LogInformation($"Order submitted {JsonSerializer.Serialize(context.Message)}");
        return Task.CompletedTask;
    }
}