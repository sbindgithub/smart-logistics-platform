using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartLogistics.Domain.Common;
using SmartLogistics.Domain.Orders;
using SmartLogistics.Domain.Orders.Repositories;
using SmartLogistics.Infrastructure.Common.Events;

namespace SmartLogistics.Infrastructure.Persistence;

public sealed class SmartLogisticsDbContext
    : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    public SmartLogisticsDbContext(
      DbContextOptions<SmartLogisticsDbContext> options,
      IMediator mediator)
      : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<Order> Orders => Set<Order>();

    public override async Task<int> SaveChangesAsync(
    CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        await DispatchDomainEventsAsync(cancellationToken);

        return result;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<DomainEvent>();

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(SmartLogisticsDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
    {
        var domainEntities = ChangeTracker
            .Entries()
            .Where(e => e.Entity is SmartLogistics.Domain.Orders.Order)
            .Select(e => e.Entity as SmartLogistics.Domain.Orders.Order)
            .Where(e => e!.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e!.DomainEvents)
            .ToList();

        domainEntities.ForEach(e => e!.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            var notification = CreateNotification(domainEvent);
            await _mediator.Publish(notification, cancellationToken);
        }
    }

    private static INotification CreateNotification(
        SmartLogistics.Domain.Common.DomainEvent domainEvent)
    {
        var notificationType = typeof(DomainEventNotification<>)
            .MakeGenericType(domainEvent.GetType());

        return (INotification)Activator.CreateInstance(
            notificationType, domainEvent)!;
    }
}
