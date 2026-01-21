using SmartLogistics.Domain.Orders;

namespace SmartLogistics.Application.Orders;

public interface IOrderService
{
    void CreateOrder(Order order);
}
