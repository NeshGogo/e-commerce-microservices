using Product.Service.Endpoints;
using Product.Service.Infrastructure.Data.EntityFramework;
using ECommerce.Shared.Infrastructure.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServerDatastore(builder.Configuration);
builder.Services.AddRabbitMqEventBus(builder.Configuration)
                .AddRabbitMqEventPublisher();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MigrateDatabase();
}

app.RegisterEndpoints();

app.Run();
