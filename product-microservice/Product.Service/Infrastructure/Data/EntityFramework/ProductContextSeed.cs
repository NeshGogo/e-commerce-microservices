using Microsoft.EntityFrameworkCore;

namespace Product.Service.Infrastructure.Data.EntityFramework;

public static class ProductContextSeed
{
    public static void MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<ProductContext>();
        context.Database.Migrate();
    }
}
