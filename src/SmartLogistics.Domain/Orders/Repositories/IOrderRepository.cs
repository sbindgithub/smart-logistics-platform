using SmartLogistics.Domain.Orders;

namespace SmartLogistics.Domain.Orders.Repositories;

public interface IOrderRepository
{
    IUnitOfWork UnitOfWork { get; }

    Task AddAsync(Order order, CancellationToken cancellationToken);
    Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken);
}
