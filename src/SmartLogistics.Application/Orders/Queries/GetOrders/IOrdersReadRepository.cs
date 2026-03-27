using System.Linq;
using SmartLogistics.Domain.Orders;

namespace SmartLogistics.Application.Orders.Queries.GetOrders;

public interface IOrdersReadRepository
{
    IQueryable<Order> Orders { get; }
}
