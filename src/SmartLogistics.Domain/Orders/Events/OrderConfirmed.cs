using SmartLogistics.Domain.Common;

namespace SmartLogistics.Domain.Orders.Events;

public sealed record OrderConfirmed(Guid OrderId) : DomainEvent;