using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLogistics.Application.Orders.Queries.GetOrders
{
    public sealed record GetOrdersQuery()
    : IRequest<IReadOnlyList<OrderListItemDto>>;
}
