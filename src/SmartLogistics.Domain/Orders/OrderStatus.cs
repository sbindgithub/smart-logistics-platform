using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLogistics.Domain.Orders
{
    /// <summary>
    /// Represents the lifecycle states of an Order aggregate.
    /// The status controls which operations are valid at any point in time.
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// Order has been created but not yet processed.
        /// This is the initial state of every order.
        /// </summary>
        Created = 1,

        /// <summary>
        /// Order has been cancelled and can no longer progress.
        /// This is a terminal state.
        /// </summary>
        Cancelled = 2,

        /// <summary>
        /// Order has been shipped to the customer.
        /// No further modifications are allowed.
        /// </summary>
        Shipped = 3
    }

}
