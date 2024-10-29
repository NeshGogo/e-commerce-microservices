using RabbitMQ.Client;

namespace Basket.Service.Infrastructure.RabbitMq
{
    public class RabbitMqConnection : IDisposable, IRabbitMqConnection
    {
        private readonly RabbitMqOptions _options;
        private IConnection? _connection;

        public IConnection Connection => _connection;

        public RabbitMqConnection(RabbitMqOptions options)
        {
            _options = options;
            InitializeConnection();
        }

        private void InitializeConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
            };
            _connection = factory.CreateConnection();  
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
