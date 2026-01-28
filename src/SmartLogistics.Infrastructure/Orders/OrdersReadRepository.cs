using Microsoft.EntityFrameworkCore;
using SmartLogistics.Application.Orders.Queries.GetOrders;
using SmartLogistics.Domain.Orders;
using SmartLogistics.Infrastructure.Persistence;

namespace SmartLogistics.Infrastructure.Orders;

public sealed class OrdersReadRepository : IOrdersReadRepository
{
    private readonly SmartLogisticsDbContext _db;

    public OrdersReadRepository(SmartLogisticsDbContext db)
    {
        _db = db;
    }

    public IQueryable<Order> Orders => _db.Orders.AsNoTracking();
}
