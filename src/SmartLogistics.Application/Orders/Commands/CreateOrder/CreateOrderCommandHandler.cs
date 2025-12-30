using SmartLogistics.Domain.Orders;
using SmartLogistics.Application.Orders.Abstractions;

namespace SmartLogistics.Application.Orders.Commands.CreateOrder
{
    /// <summary>
    /// Handles the CreateOrder command.
    /// This handler represents a WRITE use case in the Orders bounded context
    /// and is responsible for creating and persisting a new Order aggregate.
    /// </summary>
    public class CreateOrderCommandHandler
    {
        private readonly IOrderRepository _orderRepository;

        /// <summary>
        /// Initializes a new instance of the CreateOrderCommandHandler.
        /// The repository abstraction is injected to decouple the
        /// application layer from persistence details.
        /// </summary>
        public CreateOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Executes the CreateOrder command.
        /// This method acts as the transaction boundary for the use case.
        /// </summary>
        /// <param name="command">
        /// The command containing all data required to create an order.
        /// </param>
        /// <returns>
        /// A result object containing the identifier of the newly created order.
        /// </returns>
        public async Task<CreateOrderResult> Handle(CreateOrderCommand command)
        {
            // Create a new Order aggregate using domain logic.
            // Business rules and invariants are enforced inside the aggregate.
            var order = Order.Create(command.CustomerId);

            // Persist the aggregate.
            // The repository encapsulates database access and transactions.
            await _orderRepository.AddAsync(order, CancellationToken.None);

            // Return a lightweight result.
            // Command handlers should not return domain entities.
            return new CreateOrderResult(order.Id);
        }
    }
}
