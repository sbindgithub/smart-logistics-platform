using SmartLogistics.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLogistics.Application.Orders.Queries.GetOrders
{
    public sealed record OrderListItemDto(
    Guid Id,
    string OrderNumber,
    OrderStatus Status,
    DateTime CreatedAt);
}
