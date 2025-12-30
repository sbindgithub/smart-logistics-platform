using Microsoft.AspNetCore.Mvc;
using SmartLogistics.Application.Orders.Commands.CreateOrder;
using SmartLogistics.API.Models;

namespace SmartLogistics.API.Controllers
{
    /// <summary>
    /// API endpoints for managing Orders.
    /// Acts as a thin HTTP layer that delegates
    /// commands and queries to the Application layer.
    /// </summary>
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly CreateOrderCommandHandler _createOrderHandler;

        public OrdersController(CreateOrderCommandHandler createOrderHandler)
        {
            _createOrderHandler = createOrderHandler;
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            var command = new CreateOrderCommand(request.CustomerId);

            var result = await _createOrderHandler.Handle(command);

            return CreatedAtAction(nameof(GetById), new { id = result.OrderId }, result);
        }

        /// <summary>
        /// Gets an order by its identifier.
        /// Query implementation will be added next.
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            // Placeholder – Query side comes next
            return Ok();
        }
    }
}
