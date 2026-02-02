using MediatR;
using SmartLogistics.Domain.Orders.Events;
using SmartLogistics.Infrastructure.Common.Events;

namespace SmartLogistics.Infrastructure.Orders.EventHandlers;

public sealed class OrderConfirmedHandler
    : INotificationHandler<DomainEventNotification<OrderConfirmed>>
{
    public Task Handle(
        DomainEventNotification<OrderConfirmed> notification,
        CancellationToken cancellationToken)
    {
        var orderId = notification.DomainEvent.OrderId;

        // Infrastructure concerns go here:
        // - logging
        // - publishing integration events
        // - calling external systems
        // - updating projections

        return Task.CompletedTask;
    }
}