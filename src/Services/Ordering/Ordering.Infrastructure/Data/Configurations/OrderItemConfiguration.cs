
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;

namespace Ordering.Infrastructure.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        // this setup the id property of order item as a primary key.
        builder.HasKey(oi => oi.Id);
        builder.Property(oi => oi.Id).HasConversion(
            orderItemId => orderItemId.Value, // saving db
            dbId => OrderItemId.Of(dbId));    // reading db

        // foreign key 
        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(oi => oi.ProductId);

        builder.Property(oi => oi.Quantity).IsRequired();
        builder.Property(oi => oi.Price).IsRequired();
    }
}
