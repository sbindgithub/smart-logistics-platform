using SmartLogistics.Domain.Orders;
using Xunit;

namespace SmartLogistics.Domain.Tests.Orders;

public class OrderTests
{
    [Fact]
    public void Create_WithNoItems_ThrowsInvalidOperationException()
    {
        // Arrange
        var orderNumber = "ORD-001";
        var customerId = Guid.NewGuid();
        var items = Array.Empty<(Guid ProductId, int Quantity)>();

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() =>
            Order.Create(orderNumber, customerId, items));

        // Assert
        Assert.Equal(
            "Order must contain at least one item.",
            exception.Message);
    }

    [Fact]
    public void Create_WithEmptyOrderNumber_ThrowsArgumentException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var items = new[] { (Guid.NewGuid(), 1) };

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            Order.Create("", customerId, items));

        // Assert
        Assert.Equal("orderNumber", exception.ParamName);
        Assert.Contains("cannot be empty", exception.Message);
    }


}
