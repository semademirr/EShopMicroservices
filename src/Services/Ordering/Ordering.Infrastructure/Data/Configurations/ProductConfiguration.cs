
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;

namespace Ordering.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        // this is custom conversion in order to convert the product
        // id value object to the database weight value.
        builder.Property(p => p.Id).HasConversion(
            productId => productId.Value,
            dbId => ProductId.Of(dbId));
        builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
    }
}
