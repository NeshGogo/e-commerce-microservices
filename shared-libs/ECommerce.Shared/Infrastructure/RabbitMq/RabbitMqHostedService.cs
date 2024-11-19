using ECommerce.Shared.Infrastructure.EventBus;
using ECommerce.Shared.Infrastructure.EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System.Text;
using System.Diagnostics;
using OpenTelemetry.Context.Propagation;
using ECommerce.Shared.Observability;

namespace ECommerce.Shared.Infrastructure.RabbitMq;

public class RabbitMqHostedService : IHostedService
{
    private const string ExchangeName = "ecommerce-exchange";

    private readonly IServiceProvider _serviceProvider;
    private readonly EventHandlerRegistration _handlerRegistration;
    private readonly EventBusOptions _eventBusOptions;
    private readonly ActivitySource _activitySource;
    private readonly TextMapPropagator _propagator = Propagators.DefaultTextMapPropagator;

    public RabbitMqHostedService(
        IServiceProvider serviceProvider, 
        IOptions<EventHandlerRegistration> handlerRegistrations, 
        IOptions<EventBusOptions> eventBusOptions,
        RabbitMqTelemetry rabbitMqTelemetry)
    {
        _serviceProvider = serviceProvider;
        _handlerRegistration = handlerRegistrations.Value;
        _eventBusOptions = eventBusOptions.Value;
        _activitySource = rabbitMqTelemetry.ActivitySource;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = Task.Factory.StartNew(() =>
        {
            var rabbitMqConnection = _serviceProvider.GetRequiredService<IRabbitMqConnection>();

            var channel = rabbitMqConnection.Connection.CreateModel();

            channel.ExchangeDeclare(
                exchange: ExchangeName,
                type: ExchangeType.Fanout,
                durable: false,
                autoDelete: false,
                arguments: null);

            channel.QueueDeclare(
                queue: _eventBusOptions.QueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += OnMessageReceived;

            channel.BasicConsume(
                queue: _eventBusOptions.QueueName,
                autoAck: true,
                consumer: consumer,
                consumerTag: string.Empty,
                noLocal: false,
                exclusive: false,
                arguments: null);

            foreach (var (eventName, _) in _handlerRegistration.EventTypes)
            {
                channel.QueueBind(
                    queue: _eventBusOptions.QueueName,
                    exchange: ExchangeName,
                    routingKey: eventName,
                    arguments: null);
            }
        }, 
        TaskCreationOptions.LongRunning);

        return Task.CompletedTask;
    }

    private void OnMessageReceived(object? sender, BasicDeliverEventArgs eventArgs)
    {
        var parentContext = _propagator.Extract(default, eventArgs.BasicProperties,
            (properties, key) =>
            {
                if(properties.Headers.TryGetValue(key, out var value))
                {
                    var bytes = value as byte[];
                    return [Encoding.UTF8.GetString(bytes)];
                }

                return [];
            });

        var activityName = $"{OpentelemetryMessagingConventions.ReceiveOperation} {eventArgs.RoutingKey}";

        using var activity = _activitySource.StartActivity(activityName, ActivityKind.Client, 
            parentContext.ActivityContext);

        SetActivityContext(activity, eventArgs.RoutingKey, OpentelemetryMessagingConventions.ReceiveOperation);

        var eventName = eventArgs.RoutingKey;
        var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

        activity?.SetTag("message", message);

        using var scope = _serviceProvider.CreateScope();

        if (!_handlerRegistration.EventTypes.TryGetValue(eventName, out var eventType))
        {
            return;
        }

        var @event = JsonSerializer.Deserialize(message, eventType) as Event;
        foreach (var handler in
            scope.ServiceProvider.GetKeyedServices<IEventHandler>(eventType))
        {
            handler.Handle(@event);
        }
    }

    private static void SetActivityContext(Activity? activity, string routingKey, string operation)
    {
        if (activity == null) return;

        activity.SetTag(OpentelemetryMessagingConventions.System, "rabbitmq");
        activity.SetTag(OpentelemetryMessagingConventions.OperationName, operation);
        activity.SetTag(OpentelemetryMessagingConventions.DestinationName, routingKey);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
