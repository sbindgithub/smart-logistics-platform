using MediatR;
using SmartLogistics.Application.Common.Exceptions;
using SmartLogistics.Domain.Orders.Repositories;

namespace SmartLogistics.Application.Orders.Commands.ConfirmOrder;

public sealed class ConfirmOrderCommandHandler
    : IRequestHandler<ConfirmOrderCommand>
{
    private readonly IOrderRepository _orderRepository;

    public ConfirmOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Handle(ConfirmOrderCommand request, CancellationToken ct)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, ct);

        if (order == null)
            throw new NotFoundException("Order not found");

        order.Confirm();

        await _orderRepository.UnitOfWork.SaveChangesAsync(ct);
    }

}
