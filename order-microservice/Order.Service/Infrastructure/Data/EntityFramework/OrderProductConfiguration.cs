using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Service.Models;

namespace Order.Service.Infrastructure.Data.EntityFramework;

public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
{
    void IEntityTypeConfiguration<OrderProduct>.Configure(EntityTypeBuilder<OrderProduct> builder)
    {
        builder.HasKey(p => new { p.ProductId, p.OrderId });
    }
}
