using Microsoft.EntityFrameworkCore;

namespace Ordering.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.ProductId).IsRequired();
        builder.Property(o => o.Quantity).IsRequired();
        builder.Property(o => o.Price).IsRequired();
    }
}