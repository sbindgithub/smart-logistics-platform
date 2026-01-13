using Microsoft.EntityFrameworkCore;
using SmartLogistics.Domain.Orders;
using SmartLogistics.Domain.Orders.Repositories;

namespace SmartLogistics.Infrastructure.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrdersDbContext _dbContext;

    public OrderRepository(OrdersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IUnitOfWork UnitOfWork => _dbContext;

    public async Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        await _dbContext.Orders.AddAsync(order, cancellationToken);
    }

    public async Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        return await _dbContext.Orders
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
    }
}
