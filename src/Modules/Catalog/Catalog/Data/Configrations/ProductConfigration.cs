using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Data.Configrations;

public class ProductConfigration: IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Category).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(100);
        builder.Property(p => p.ImageFile).HasMaxLength(100);
        builder.Property(p => p.Price).IsRequired();
    }
}