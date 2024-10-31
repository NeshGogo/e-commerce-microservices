using RabbitMQ.Client;

namespace ECommerce.Shared.Infrastructure.RabbitMq;

public interface IRabbitMqConnection
{
    public IConnection Connection { get; }
}
