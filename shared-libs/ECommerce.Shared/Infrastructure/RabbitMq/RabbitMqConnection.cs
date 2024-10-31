using RabbitMQ.Client;

namespace ECommerce.Shared.Infrastructure.RabbitMq;

public class RabbitMqConnection : IDisposable, IRabbitMqConnection
{
    private IConnection? _connection;
    private readonly RabbitMqOptions _options;

    public IConnection Connection => throw new NotImplementedException();

    public RabbitMqConnection(RabbitMqOptions options)
    {
        _options = options;
        InitializeConnection();
    }

    private void InitializeConnection()
    {
        var fabric = new ConnectionFactory
        {
            HostName = _options.HostName,
        };
        _connection = fabric.CreateConnection();
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}
