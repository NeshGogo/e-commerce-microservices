using Microsoft.EntityFrameworkCore;

namespace ECommerce.Shared.Infrastructure.Outbox;

public class OutboxContext : DbContext
{
    public OutboxContext(DbContextOptions<OutboxContext> options)
        : base(options)
    {        
    }

    public DbSet<OutBoxEvent> OutBoxEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OutboxEventConfiguration());
    }
}
