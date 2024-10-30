using Order.Service.Infrastructure.EventBus;
using Order.Service.Infrastructure.EventBus.Abstractions;
using RabbitMQ.Client;
using System.Text.Json;

namespace Order.Service.Infrastructure.RabbitMq;

public class RabbitMqEventBus : IEventBus
{
    private const string ExchangeName = "ecommerce-exchange";
    private readonly IRabbitMqConnection _rabbitMqConnection;

    public RabbitMqEventBus(IRabbitMqConnection rabbitMqConnection)
    {
        _rabbitMqConnection = rabbitMqConnection;
    }

    public Task PublishAsync(Event @event)
    {
        using var channel = _rabbitMqConnection.Connection.CreateModel();

        channel.ExchangeDeclare(
            exchange: ExchangeName,
            type: ExchangeType.Fanout,
            durable: false,
            autoDelete: false,
            arguments: null);

        var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType());

        channel.BasicPublish(
            exchange: ExchangeName,
            routingKey: string.Empty,
            mandatory: false,
            basicProperties: null,
            body: body);

        return Task.CompletedTask;
    }
}
