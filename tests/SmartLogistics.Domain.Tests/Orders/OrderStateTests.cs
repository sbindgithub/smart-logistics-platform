using SmartLogistics.Domain.Orders;
using Xunit;

namespace SmartLogistics.Domain.Tests.Orders;

public class OrderStateTests
{
    private static Order CreateValidOrder()
    {
        return Order.Create(
            orderNumber: "ORD-STATE-001",
            customerId: Guid.NewGuid(),
            items: new[] { (Guid.NewGuid(), 1) });
    }

    [Fact]
    public void NewOrder_HasStatusCreated()
    {
        // Arrange & Act
        var order = CreateValidOrder();

        // Assert
        Assert.Equal(OrderStatus.Created, order.Status);
    }

    [Fact]
    public void Confirm_FromCreated_ChangesStatusToConfirmed()
    {
        // Arrange
        var order = CreateValidOrder();

        // Act
        order.Confirm();

        // Assert
        Assert.Equal(OrderStatus.Confirmed, order.Status);
    }

    [Fact]
    public void Confirm_WhenAlreadyConfirmed_ThrowsInvalidOperationException()
    {
        // Arrange
        var order = CreateValidOrder();
        order.Confirm();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            order.Confirm());

        Assert.Contains("Only created orders can be confirmed", exception.Message);
    }

    [Fact]
    public void Cancel_FromCreated_ChangesStatusToCancelled()
    {
        // Arrange
        var order = CreateValidOrder();

        // Act
        order.Cancel();

        // Assert
        Assert.Equal(OrderStatus.Cancelled, order.Status);
    }

    [Fact]
    public void Cancel_WhenAlreadyCancelled_ThrowsInvalidOperationException()
    {
        // Arrange
        var order = CreateValidOrder();
        order.Cancel();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            order.Cancel());

        Assert.Contains("already cancelled", exception.Message);
    }

    [Fact]
    public void Cancel_AfterConfirm_ThrowsInvalidOperationException()
    {
        // Arrange
        var order = CreateValidOrder();
        order.Confirm();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            order.Cancel());

        Assert.Contains(
    "confirmed orders cannot be cancelled",
    exception.Message,
    StringComparison.OrdinalIgnoreCase);

    }

    [Fact]
    public void Confirm_AfterCancel_ThrowsInvalidOperationException()
    {
        // Arrange
        var order = CreateValidOrder();
        order.Cancel();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            order.Confirm());

        Assert.Contains(
            "cancelled orders cannot be confirmed",
            exception.Message,
            StringComparison.OrdinalIgnoreCase);
    }
    [Fact]
    public void Confirm_RaisesOrderConfirmedEvent()
    {
        // Arrange
        var order = CreateValidOrder();

        // Act
        order.Confirm();

        // Assert
        var domainEvent = Assert.Single(order.DomainEvents);
        Assert.IsType<SmartLogistics.Domain.Orders.Events.OrderConfirmed>(domainEvent);
    }

    [Fact]
    public void Cancel_RaisesOrderCancelledEvent()
    {
        // Arrange
        var order = CreateValidOrder();

        // Act
        order.Cancel();

        // Assert
        var domainEvent = Assert.Single(order.DomainEvents);
        Assert.IsType<SmartLogistics.Domain.Orders.Events.OrderCancelled>(domainEvent);
    }
}
