using Microsoft.EntityFrameworkCore;
using SmartLogistics.Application.Orders.Abstractions;
using SmartLogistics.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLogistics.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrdersDbContext _dbContext;

        public OrderRepository(OrdersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Order order, CancellationToken cancellationToken)
        {
            await _dbContext.Orders.AddAsync(order, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken)
        {
            return await _dbContext.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
        }
    }
}
