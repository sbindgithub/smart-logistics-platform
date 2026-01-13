namespace SmartLogistics.Domain.Orders;

public class OrderItem
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }

    // Required by EF Core
    private OrderItem() { }

    internal OrderItem(Guid productId, int quantity)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("ProductId cannot be empty.", nameof(productId));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        ProductId = productId;
        Quantity = quantity;
    }
}
