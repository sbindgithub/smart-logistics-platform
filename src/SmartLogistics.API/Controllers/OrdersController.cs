using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartLogistics.Application.Orders.Commands.CreateOrder;
using SmartLogistics.Application.Orders.Commands.ConfirmOrder;
using SmartLogistics.Application.Orders.Queries.GetOrders;

namespace SmartLogistics.Api.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new order.
    /// </summary>
    /// <remarks>
    /// Returns 201 Created with the order id.
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateOrderCommand command,
        CancellationToken ct)
    {
        var orderId = await _mediator.Send(command, ct);

        return CreatedAtAction(
            nameof(GetById),
            new { id = orderId },
            new { id = orderId });
    }

    /// <summary>
    /// Confirms an existing order.
    /// </summary>
    /// <remarks>
    /// Returns:
    /// 204 - success  
    /// 404 - order not found  
    /// 409 - concurrency conflict  
    /// </remarks>
    [HttpPost("{id:guid}/confirm")]
    public async Task<IActionResult> Confirm(Guid id, CancellationToken ct)
    {
        await _mediator.Send(new ConfirmOrderCommand(id), ct);
        return NoContent();
    }

    /// <summary>
    /// Simple liveness endpoint.
    /// </summary>
    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok("Orders API is alive");
    }

    /// <summary>
    /// Placeholder for GetById (to be implemented next).
    /// </summary>
    /// <remarks>
    /// This exists so CreatedAtAction works correctly.
    /// </remarks>
    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        // Will be replaced with a proper query handler next
        return Ok(new { id });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var orders = await _mediator.Send(new GetOrdersQuery(), ct);
        return Ok(orders);
    }

   

}
