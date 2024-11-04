namespace Basket.Service.Infrastructure.Redis;

public class RedisOptions
{
    public const string RedisSectionName = "Redis";
    public string Configuration { get; set; } = string.Empty;
}
