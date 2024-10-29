
using Basket.Service.Infrastructure.Data;
using Basket.Service.IntegrationEvents;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

namespace Basket.Service.Infrastructure.RabbitMq;

public class RabbitMqHostedService : IHostedService
{
    private IServiceProvider _serviceProvider;

    public RabbitMqHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = Task.Factory.StartNew(() =>
        {
            var rabbitMqConnection = _serviceProvider.GetRequiredService<IRabbitMqConnection>();

            var channel = rabbitMqConnection.Connection.CreateModel();

            channel.QueueDeclare(
                queue: nameof(OrderCreatedEvent),
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += OnMessageReceived;

            channel.BasicConsume(
                queue: nameof(OrderCreatedEvent),
                autoAck: true,
                consumerTag: nameof(OrderCreatedEvent),
                noLocal: false,
                exclusive: false,
                arguments: null,
                consumer: consumer
                );

        }, 
        TaskCreationOptions.LongRunning);

        return Task.CompletedTask;
    }

    private void OnMessageReceived(object? sender, BasicDeliverEventArgs e)
    {
        var message = Encoding.UTF8.GetString(e.Body.Span);

        var @event = JsonSerializer.Deserialize<OrderCreatedEvent>(message);

        using var scope = _serviceProvider.CreateScope();

        var basketStore = scope.ServiceProvider.GetRequiredService<IBasketStore>();

        basketStore.DeleteCustomerBasket(@event.CustomerId);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
