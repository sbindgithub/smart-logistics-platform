using Microsoft.EntityFrameworkCore;
using SmartLogistics.Domain.Orders;
using SmartLogistics.Domain.Orders.Repositories;

namespace SmartLogistics.Infrastructure.Persistence
{
    /// <summary>
    /// EF Core DbContext responsible for persisting Order aggregates.
    /// This DbContext represents the WRITE model of the Orders bounded context
    /// and is used exclusively by command handlers.
    /// </summary>
    public class OrdersDbContext : DbContext, IUnitOfWork
    {
        public DbSet<Order> Orders => Set<Order>();

        public OrdersDbContext(DbContextOptions<OrdersDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(OrdersDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }

}
