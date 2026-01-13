using MediatR;
using SmartLogistics.Domain.Orders;
using SmartLogistics.Domain.Orders.Repositories;

namespace SmartLogistics.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Guid> Handle(
    CreateOrderCommand request,
    CancellationToken cancellationToken)
    {
        var order = Order.Create(
            request.OrderNumber,
            request.CustomerId,
            request.Items.Select(i => (i.ProductId, i.Quantity)));

        await _orderRepository.AddAsync(order, cancellationToken);
        await _orderRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return order.Id;
    }

}
