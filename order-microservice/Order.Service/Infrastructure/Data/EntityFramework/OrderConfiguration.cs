using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Order.Service.Infrastructure.Data.EntityFramework;

internal class OrderConfiguration : IEntityTypeConfiguration<Models.Order>
{
    public void Configure(EntityTypeBuilder<Models.Order> builder)
    {
        builder.HasKey(x => x.OrderId);
        builder.Property(x => x.CustomerId).IsRequired();
        builder.HasMany(x => x.OrderProducts).WithOne();
    }
}
