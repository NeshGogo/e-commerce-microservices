namespace Basket.Service.Infrastructure.RabbitMq;

public static class RabbitMqExtension
{
    public static void AddRabbitMqConnection(this IServiceCollection services, IConfiguration configuration)
    {
        RabbitMqOptions opts = new();
        configuration.GetSection(RabbitMqOptions.RabbitMqSectionName).Bind(opts);

        services.AddSingleton<IRabbitMqConnection>(new RabbitMqConnection(opts));
    }
}
