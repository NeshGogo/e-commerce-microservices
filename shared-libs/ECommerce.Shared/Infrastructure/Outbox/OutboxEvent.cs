namespace ECommerce.Shared.Infrastructure.Outbox;

public class OutBoxEvent
{
    public Guid Id { get; set; }
    public required string EventType { get; set; }
    public required string Data { get; set; }
    public bool Sent { get; set; }
}
