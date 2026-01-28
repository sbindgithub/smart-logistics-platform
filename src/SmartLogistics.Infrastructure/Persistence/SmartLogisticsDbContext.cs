using Microsoft.EntityFrameworkCore;
using SmartLogistics.Domain.Orders;
using SmartLogistics.Domain.Orders.Repositories;

namespace SmartLogistics.Infrastructure.Persistence;

public sealed class SmartLogisticsDbContext
    : DbContext, IUnitOfWork
{
    public SmartLogisticsDbContext(
        DbContextOptions<SmartLogisticsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();

    public override Task<int> SaveChangesAsync(
        CancellationToken ct = default)
    {
        return base.SaveChangesAsync(ct);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(SmartLogisticsDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
