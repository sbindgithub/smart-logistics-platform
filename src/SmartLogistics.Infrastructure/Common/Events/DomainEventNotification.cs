using MediatR;
using SmartLogistics.Domain.Common;

namespace SmartLogistics.Infrastructure.Common.Events;

/// <summary>
/// Adapts a DomainEvent to a MediatR notification
/// </summary>
public sealed record DomainEventNotification<TDomainEvent>(
    TDomainEvent DomainEvent
) : INotification
    where TDomainEvent : DomainEvent;