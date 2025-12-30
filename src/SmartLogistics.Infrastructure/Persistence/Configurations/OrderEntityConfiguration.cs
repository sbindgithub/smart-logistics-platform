using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLogistics.Domain.Orders;

namespace SmartLogistics.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// EF Core configuration for the Order aggregate.
    /// This class defines how the Order domain model is mapped
    /// to the relational database schema.
    /// </summary>
    public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        /// <summary>
        /// Configures the database mapping for the Order aggregate.
        /// This method is invoked by EF Core during model creation.
        /// </summary>
        /// <param name="builder">
        /// Provides a fluent API for configuring entity properties,
        /// keys, constraints, and table mappings.
        /// </param>
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Maps the Order aggregate to the "Orders" table.
            // Explicit table naming avoids EF Core conventions leaking
            // into the physical database design.
            builder.ToTable("Orders");

            // Defines the primary key for the Order aggregate.
            // The Id represents the aggregate identity and must be unique.
            builder.HasKey(o => o.Id);

            // Maps the Order status.
            // This field is required to enforce valid lifecycle states
            // (e.g., Created, Cancelled, Shipped).
            builder.Property(o => o.Status)
                   .IsRequired();

            // Records the creation timestamp of the Order.
            // This is mandatory for auditing, tracing, and event correlation.
            builder.Property(o => o.CreatedAt)
                   .IsRequired();
        }
    }
}
