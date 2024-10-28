namespace Order.Service.Infrastructure.RabbitMq;

public class RabbitMqOptions
{
    public const string RabbitMqSecctionName = "RabbitMq";
    public string HostName { get; set; } = string.Empty;
}
