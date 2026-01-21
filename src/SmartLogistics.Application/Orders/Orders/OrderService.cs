using SmartLogistics.Application.Common;
using SmartLogistics.Domain.Orders;

namespace SmartLogistics.Application.Orders;

public sealed class OrderService : IOrderService
{
    private readonly IAppLogger<OrderService> _logger;

    public OrderService(IAppLogger<OrderService> logger)
    {
        _logger = logger;
    }

    public void CreateOrder(Order order)
    {
        _logger.Info("Order creation started");
        _logger.Info("Order creation completed");
    }
}
