using Microsoft.EntityFrameworkCore;

namespace Order.Service.Infrastructure.Data.EntityFramework;

public static class OrderContextSeed
{
    public static void MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<OrderContext>();
        context.Database.Migrate();
    }
}
