using ECommerce.Shared.Infrastructure.RabbitMq;
using Order.Service.Endpoints;
using Order.Service.Infrastructure.Data.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServerDatastore(builder.Configuration);
builder.Services.AddRabbitMqEventBus(builder.Configuration);
builder.Services.AddRabbitMqEventPublisher();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MigrateDatabase();
}

app.RegisterEndpoints();

app.Run();
