using ECommerce.Shared.Infrastructure.Outbox;
using ECommerce.Shared.Infrastructure.RabbitMq;
using ECommerce.Shared.Observability;
using OpenTelemetry.Metrics;
using Order.Service.Endpoints;
using Order.Service.Infrastructure.Data.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

var serviceName = "Order";

builder.Services.AddSqlServerDatastore(builder.Configuration);
builder.Services.AddRabbitMqEventBus(builder.Configuration);
builder.Services.AddRabbitMqEventPublisher();
builder.Services.AddOpenTelemetryTracing(serviceName, builder.Configuration, (tracing) => tracing.WithSqlInstrumentation())
                .AddOpenTelemetryMetrics(serviceName, builder.Services, (metricBuilder) =>
                {
                    metricBuilder.AddView("products-per-order", new ExplicitBucketHistogramConfiguration
                    {
                        Boundaries = [1, 2, 5, 10]
                    });
                });
builder.Services.AddOutbox(builder.Configuration);
    

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MigrateDatabase();
    app.ApplyOutboxMigrations();
}

app.RegisterEndpoints();
app.UsePrometheusExporter();

app.Run();

public partial class Program { }