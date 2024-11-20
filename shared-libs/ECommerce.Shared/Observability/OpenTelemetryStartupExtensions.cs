using ECommerce.Shared.Infrastructure.RabbitMq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ECommerce.Shared.Observability;

public static class OpenTelemetryStartupExtensions
{
    public static OpenTelemetryBuilder AddOpenTelemetryTracing(
        this IServiceCollection services,
        string serviceName,
        IConfigurationManager configuration,
        Action<TracerProviderBuilder>? customTracing = null)
    {
        OpenTelemetryOptions openTelemetryOptions = new();
        configuration.GetSection(OpenTelemetryOptions.OpenTelemetrySectionName).Bind(openTelemetryOptions);

        return services.AddOpenTelemetry()
                    .ConfigureResource(r =>
                        r.AddService(serviceName))
                         .WithTracing(builder =>
                         {
                             builder
                                 .AddConsoleExporter()
                                 .AddAspNetCoreInstrumentation()
                                 .AddSource(RabbitMqTelemetry.ActivitySourceName)
                                 .AddOtlpExporter(options =>
                                 {
                                     options.Endpoint = new Uri(openTelemetryOptions.OtlpExporterEndpoint);
                                 });

                             customTracing?.Invoke(builder);
                         });
    }


    public static TracerProviderBuilder WithSqlInstrumentation(this TracerProviderBuilder builder) =>
        builder.AddSqlClientInstrumentation();

    public static OpenTelemetryBuilder AddOpenTelemetryMetrics(this OpenTelemetryBuilder openTelemetryBuilder, 
            string serviceMame, IServiceCollection services, Action<MeterProviderBuilder>? customMetrics = null)
    {
        services.AddSingleton(new MetricFactory(serviceMame));

        return openTelemetryBuilder.WithMetrics(builder =>
        {
            builder.AddConsoleExporter()
                   .AddAspNetCoreInstrumentation()
                   .AddMeter(serviceMame);

            customMetrics?.Invoke(builder);
        });
    } 
        
}
