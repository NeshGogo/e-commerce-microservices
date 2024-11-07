using Microsoft.EntityFrameworkCore;

namespace Order.Service.Infrastructure.Data.EntityFramework;

public static class EntityFrameworkExtensions
{
    public static void AddSqlServerDatastore(this IServiceCollection services, IConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        services.AddDbContext<OrderContext>(opt => opt.UseSqlServer(connectionString));

        services.AddScoped<IOrderStore, OrderContext>();
    }
}
