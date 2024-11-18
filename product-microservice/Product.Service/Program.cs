using Product.Service.Endpoints;
using Product.Service.Infrastructure.Data.EntityFramework;
using ECommerce.Shared.Infrastructure.RabbitMq;
using ECommerce.Shared.Observability;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServerDatastore(builder.Configuration);
builder.Services.AddRabbitMqEventBus(builder.Configuration)
                .AddRabbitMqEventPublisher();
builder.Services.AddOpenTelemetryTracing("Product", traceBuilder => traceBuilder.WithSqlInstrumentation());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MigrateDatabase();
}

app.RegisterEndpoints();

app.Run();

public partial class Program { }