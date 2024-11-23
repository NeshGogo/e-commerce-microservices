using ECommerce.Shared.Infrastructure.EventBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text.Json;

namespace ECommerce.Shared.Infrastructure.Outbox;

public class OutboxContext : DbContext, IOutboxStore
{
    public OutboxContext(DbContextOptions<OutboxContext> options)
        : base(options)
    {        
    }

    public DbSet<OutboxEvent> OutBoxEvents { get; set; }

    public async Task AddOutboxEvent<T>(T @event) where T : Event
    {
        var existingEvent = await OutBoxEvents.FindAsync(@event.Id);
        if (existingEvent != null) return;

        OutBoxEvents.Add(new OutboxEvent
        {
            Id = @event.Id,
            EventType = @event.GetType().AssemblyQualifiedName,
            Data = JsonSerializer.Serialize(@event)
        });

        await SaveChangesAsync();
    }

    public IExecutionStrategy CreateExecutionStrategy() => 
        Database.CreateExecutionStrategy();

    public Task<List<OutboxEvent>> GetUnpublishedOutboxEvents() =>
        OutBoxEvents.Where(p => !p.Sent).ToListAsync();

    public async Task MarkOutboxEventAsPublished(Guid outBoxEventId)
    {
        var outboxEvent = await OutBoxEvents.FindAsync(outBoxEventId);

        if(outboxEvent is not null)
        {
            outboxEvent.Sent = true;
            await SaveChangesAsync();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OutboxEventConfiguration());
    }
}
