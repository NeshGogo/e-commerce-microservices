using ECommerce.Shared.Infrastructure.EventBus.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Shared.Infrastructure.RabbitMq;

public static class RabbitMqStartupExtensions
{
    public static IServiceCollection AddRabbitMqEventBus(this IServiceCollection services, IConfigurationManager configuration)
    {
        RabbitMqOptions options = new();        
        configuration.GetSection(RabbitMqOptions.RabbitMqSectionName).Bind(options);
        
        services.AddSingleton<IRabbitMqConnection>(new RabbitMqConnection(options));

        return services;
    }

    public static IServiceCollection AddRabbitMqEventPublisher(this IServiceCollection services)
    {
        services.AddScoped<IEventBus, RabbitMqEventBus>();

        return services;
    }
}
