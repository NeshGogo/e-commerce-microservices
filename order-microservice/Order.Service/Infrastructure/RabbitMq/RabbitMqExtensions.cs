using Order.Service.Infrastructure.EventBus.Abstractions;

namespace Order.Service.Infrastructure.RabbitMq
{
    public static class RabbitMqExtensions
    {
        public static void AddRabbitMqEventBus(this IServiceCollection services, IConfigurationManager configurationManager)
        {
            RabbitMqOptions rabbitMqOptons = new();
            configurationManager.GetSection(rabbitMqOptons.HostName).Bind(rabbitMqOptons);

            services.AddSingleton<IRabbitMqConnection>(new RabbitMqConnection(rabbitMqOptons));
            services.AddScoped<IEventBus, RabbitMqEventBus>();
        }

    }
}
