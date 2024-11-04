namespace Basket.Service.Infrastructure.Redis;

public static class RedisExtensions
{
    public static void AddRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        RedisOptions redisOptions = new();
        configuration.GetRequiredSection(RedisOptions.RedisSectionName).Bind(redisOptions);

        services.AddStackExchangeRedisCache(opt => opt.Configuration = redisOptions.Configuration);
    }
}
