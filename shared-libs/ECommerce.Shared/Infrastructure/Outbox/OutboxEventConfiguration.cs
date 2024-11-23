using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Shared.Infrastructure.Outbox
{
    public class OutboxEventConfiguration : IEntityTypeConfiguration<OutBoxEvent>
    {
        public void Configure(EntityTypeBuilder<OutBoxEvent> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.EventType).IsRequired();
            builder.Property(o => o.Data).IsRequired();
        }
    }
}
