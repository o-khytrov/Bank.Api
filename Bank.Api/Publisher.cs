using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace Bank.Api;

public interface IPublisher
{
    void Publish<T>(T request);
}

public class Publisher : IPublisher, IDisposable
{
    private readonly IConnection _rabbitConnection;

    public Publisher()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "user",
            Password = "password",
        };

        _rabbitConnection = factory.CreateConnection();
    }

    public void Publish<T>(T request)
    {
        using var channel = _rabbitConnection.CreateModel();
        channel.QueueDeclare(queue: "orders",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);


        var message = JsonSerializer.Serialize(request);
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "",
            routingKey: "orders",
            basicProperties: null,
            body: body);
    }


    public void Dispose()
    {
        _rabbitConnection.Dispose();
    }
}