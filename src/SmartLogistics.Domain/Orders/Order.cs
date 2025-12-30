using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLogistics.Domain.Orders
{
    public class Order
    {
        public Guid Id { get; private set; }
        public OrderStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Order() { } // Required by EF Core

        public static Order Create(Guid customerId)
        {
            return new Order
            {
                Id = Guid.NewGuid(),
                Status = OrderStatus.Created,
                CreatedAt = DateTime.UtcNow
            };
        }
    }

}
