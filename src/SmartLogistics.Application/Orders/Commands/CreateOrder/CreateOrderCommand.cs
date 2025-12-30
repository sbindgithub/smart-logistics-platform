namespace SmartLogistics.Application.Orders.Commands.CreateOrder
{
    /// <summary>
    /// Command representing the intent to create a new Order.
    /// 
    /// Commands are immutable and contain only the data
    /// required to execute a write operation.
    /// They do NOT contain business logic.
    /// </summary>
    public sealed class CreateOrderCommand
    {
        /// <summary>
        /// Identifier of the customer placing the order.
        /// This value is required to associate the order
        /// with its owner in the domain.
        /// </summary>
        public Guid CustomerId { get; }

        /// <summary>
        /// Initializes a new instance of CreateOrderCommand.
        /// </summary>
        /// <param name="customerId">
        /// Unique identifier of the customer.
        /// </param>
        public CreateOrderCommand(Guid customerId)
        {
            CustomerId = customerId;
        }
    }
}
