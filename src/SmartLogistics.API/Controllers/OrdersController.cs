using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartLogistics.Application.Orders.Commands.CreateOrder;

namespace SmartLogistics.API.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateOrderCommand command)
    {
        var orderId = await _mediator.Send(command);
        return CreatedAtAction(nameof(Create), new { id = orderId }, orderId);
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Orders API is alive");
    }

}
