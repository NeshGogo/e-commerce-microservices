using Microsoft.EntityFrameworkCore;

namespace Order.Service.Infrastructure.Data.EntityFramework;

internal class OrderContext : DbContext, IOrderStore
{
    public OrderContext(DbContextOptions options) 
        : base(options)
    {
    }

    public DbSet<Models.Order> Orders { get; set; }

    public async Task CreateOrderAsync(Models.Order order)
    {
        Orders.Add(order);
        await SaveChangesAsync();
    }

    public async Task<Models.Order?> GetCustomerOrderByIdAsync(string customerId, string orderId) =>
        await Orders.Include(p => p.OrderProducts)
            .SingleOrDefaultAsync(p => p.CustomerId == customerId && p.OrderId == Guid.Parse(orderId));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderProductConfiguration());
    }
}
