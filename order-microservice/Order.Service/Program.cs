using Order.Service.Endpoints;
using Order.Service.Infrastructure.Data;
using Order.Service.Infrastructure.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IOrderStore, InMemoryOrderStore>();
builder.Services.AddRabbitMqEventBus(builder.Configuration);

var app = builder.Build();

app.RegisterEndpoints();

app.Run();
