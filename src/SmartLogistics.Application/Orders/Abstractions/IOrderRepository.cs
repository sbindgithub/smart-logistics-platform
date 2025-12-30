using SmartLogistics.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLogistics.Application.Orders.Abstractions
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order, CancellationToken cancellationToken);
        Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken);
    }
}
