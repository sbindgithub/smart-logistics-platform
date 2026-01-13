namespace SmartLogistics.Domain.Orders;

public class Order
{
    private readonly List<OrderItem> _items = new();

    public Guid Id { get; private set; }
    public string OrderNumber { get; private set; } = default!;
    public Guid CustomerId { get; private set; }

    public OrderStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order() { } // EF Core

    private Order(string orderNumber, Guid customerId)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
            throw new ArgumentException("OrderNumber cannot be empty.", nameof(orderNumber));

        if (customerId == Guid.Empty)
            throw new ArgumentException("CustomerId cannot be empty.", nameof(customerId));

        Id = Guid.NewGuid();
        OrderNumber = orderNumber;
        CustomerId = customerId;

        Status = OrderStatus.Created;
        CreatedAt = DateTime.UtcNow;   // ← set ONCE, here
    }

    public static Order Create(
        string orderNumber,
        Guid customerId,
        IEnumerable<(Guid ProductId, int Quantity)> items)
    {
        var order = new Order(orderNumber, customerId);

        foreach (var item in items)
        {
            order.AddItem(item.ProductId, item.Quantity);
        }

        if (!order._items.Any())
            throw new InvalidOperationException("Order must contain at least one item.");

        return order;
    }

    public void AddItem(Guid productId, int quantity)
    {
        var orderItem = new OrderItem(productId, quantity);
        _items.Add(orderItem);
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Created)
            throw new InvalidOperationException("Only created orders can be confirmed.");

        Status = OrderStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Order is already cancelled.");

        Status = OrderStatus.Cancelled;
    }
}
