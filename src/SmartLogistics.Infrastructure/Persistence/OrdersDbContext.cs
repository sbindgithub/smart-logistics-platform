using Microsoft.EntityFrameworkCore;
using SmartLogistics.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLogistics.Infrastructure.Persistence
{
    /// <summary>
    /// EF Core DbContext responsible for persisting Order aggregates.
    /// This DbContext represents the WRITE model of the Orders bounded context
    /// and is used exclusively by command handlers.
    /// </summary>
    public class OrdersDbContext:DbContext
    {
        /// <summary>
        /// Initializes a new instance of OrdersDbContext.
        /// DbContextOptions are injected by the DI container and contain
        /// configuration such as connection string and provider (SQL Server).
        /// </summary>
        /// <param name="options">
        /// EF Core configuration options supplied at application startup.
        /// </param>
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options) // Pass options to the base DbContext
        { 
         
        }

        /// <summary>
        /// Represents the Orders table in the database.
        /// This DbSet is used to track and persist Order aggregate roots.
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// Configures the EF Core model.
        /// This method is called once when the model for this context is created.
        /// </summary>
        /// <param name="modelBuilder">
        /// Provides a fluent API to configure entity mappings and relationships.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Automatically applies all IEntityTypeConfiguration<T>
            // implementations found in this assembly.
            // This keeps domain models free from EF Core attributes
            // and centralizes persistence configuration in Infrastructure.
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrdersDbContext).Assembly);

        }
    }
}
