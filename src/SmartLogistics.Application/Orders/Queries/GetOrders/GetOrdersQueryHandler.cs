using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SmartLogistics.Application.Orders.Queries.GetOrders;

public sealed class GetOrdersQueryHandler
    : IRequestHandler<GetOrdersQuery, IReadOnlyList<OrderListItemDto>>
{
    private readonly IOrdersReadRepository _db;

    public GetOrdersQueryHandler(IOrdersReadRepository db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<OrderListItemDto>> Handle(
        GetOrdersQuery request,
        CancellationToken ct)
    {
        return await _db.Orders
            .AsNoTracking()
            .Select(o => new OrderListItemDto(
                o.Id,
                o.OrderNumber,
                o.Status,
                o.CreatedAt))
            .ToListAsync(ct);
    }
}
