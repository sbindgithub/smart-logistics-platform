using Moq;
using SmartLogistics.Application.Orders.Commands.CreateOrder;
using SmartLogistics.Application.Orders.Dtos;
using SmartLogistics.Domain.Orders;
using SmartLogistics.Domain.Orders.Repositories;
using Xunit;

namespace SmartLogistics.Application.Tests.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_PersistsOrderAndReturnsId()
    {
        // Arrange
        var orderRepositoryMock = new Mock<IOrderRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        orderRepositoryMock
            .Setup(r => r.UnitOfWork)
            .Returns(unitOfWorkMock.Object);

        var handler = new CreateOrderCommandHandler(orderRepositoryMock.Object);

        var command = new CreateOrderCommand
        {
            OrderNumber = "ORD-001",
            CustomerId = Guid.NewGuid(),
            Items = new[]
            {
                new CreateOrderItemDto
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 2
                },
                new CreateOrderItemDto
                {
                    ProductId = Guid.NewGuid(),
                    Quantity = 1
                }
            }
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        orderRepositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWorkMock.Verify(
            u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);

        Assert.NotEqual(Guid.Empty, result);
    }

    [Fact]
    public async Task Handle_InvalidCommand_DoesNotPersistOrCommit()
    {
        // Arrange
        var orderRepositoryMock = new Mock<IOrderRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        orderRepositoryMock
            .Setup(r => r.UnitOfWork)
            .Returns(unitOfWorkMock.Object);

        var handler = new CreateOrderCommandHandler(orderRepositoryMock.Object);

        // Invalid command: no items → Domain will throw
        var command = new CreateOrderCommand
        {
            OrderNumber = "ORD-002",
            CustomerId = Guid.NewGuid(),
            Items = Array.Empty<CreateOrderItemDto>()
        };

        // Act
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(command, CancellationToken.None));

        // Assert
        orderRepositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()),
            Times.Never);

        unitOfWorkMock.Verify(
            u => u.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never);
    }

}
