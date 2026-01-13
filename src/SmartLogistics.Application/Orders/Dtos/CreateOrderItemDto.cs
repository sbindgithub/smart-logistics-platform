namespace SmartLogistics.Application.Orders.Dtos;

public class CreateOrderItemDto
{
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
}
