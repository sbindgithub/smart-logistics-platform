using MediatR;
using SmartLogistics.Application.Orders.Dtos;

namespace SmartLogistics.Application.Orders.Commands.CreateOrder;

/// <summary>
/// Command representing the intent to create a new Order.
/// Commands contain only the data required to perform
/// a write operation and no business logic.
/// </summary>
public sealed class CreateOrderCommand : IRequest<Guid>
{
    /// <summary>
    /// Business order number.
    /// </summary>
    public string OrderNumber { get; init; } = default!;

    /// <summary>
    /// Identifier of the customer placing the order.
    /// </summary>
    public Guid CustomerId { get; init; }

    /// <summary>
    /// Items included in the order.
    /// </summary>
    public IReadOnlyCollection<CreateOrderItemDto> Items { get; init; }
        = Array.Empty<CreateOrderItemDto>();
}
