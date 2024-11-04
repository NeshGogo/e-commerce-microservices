using Basket.Service.Endpoints;
using Basket.Service.Infrastructure.Data;
using Basket.Service.IntegrationEvents;
using Basket.Service.IntegrationEvents.Handlers;
using ECommerce.Shared.Infrastructure.EventBus;
using ECommerce.Shared.Infrastructure.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IBasketStore, InMemoryBasketStore>();
builder.Services.AddRabbitMqEventBus(builder.Configuration)
                .AddRabbitMqSubscriberService(builder.Configuration)
                .AddEventHandler<OrderCreatedEvent, OrderCreatedEventHandler>()
                .AddEventHandler<ProductPriceUpdatedEvent, ProductPriceUpdatedEventHandler>();

var app = builder.Build();

app.RegisterEndpoints();

app.Run();
