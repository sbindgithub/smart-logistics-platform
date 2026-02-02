using SmartLogistics.Domain.Common;

namespace SmartLogistics.Domain.Orders.Events;

public sealed record OrderCancelled(Guid OrderId) : DomainEvent;