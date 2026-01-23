using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLogistics.Domain.Orders;

namespace SmartLogistics.Infrastructure.Persistence.Configurations;


public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Status)
               .IsRequired();

        builder.Property(o => o.CreatedAt)
               .IsRequired();

        // ✅ THIS IS REQUIRED
        builder.OwnsMany(o => o.Items, items =>
        {
            items.ToTable("OrderItems");

            items.WithOwner()
                 .HasForeignKey("OrderId");

            // Shadow key – required by EF, not by your domain
            items.Property<Guid>("Id");
            items.HasKey("Id");

            items.Property(i => i.ProductId)
                 .IsRequired();

            items.Property(i => i.Quantity)
                 .IsRequired();
        });
    }
}
