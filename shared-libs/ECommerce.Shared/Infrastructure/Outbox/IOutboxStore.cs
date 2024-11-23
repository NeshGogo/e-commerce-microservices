using ECommerce.Shared.Infrastructure.EventBus;
using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerce.Shared.Infrastructure.Outbox;

public interface IOutboxStore
{
    Task AddOutboxEvent<T>(T @event) where T : Event;
    Task<List<OutboxEvent>> GetUnpublishedOutboxEvents();
    Task MarkOutboxEventAsPublished(Guid outBoxEventId);
    IExecutionStrategy CreateExecutionStrategy();
}
