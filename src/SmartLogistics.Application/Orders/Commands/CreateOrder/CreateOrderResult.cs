namespace SmartLogistics.Application.Orders.Commands.CreateOrder
{
    /// <summary>
    /// Result returned after successfully creating an order.
    /// </summary>
    public class CreateOrderResult
    {
        public Guid OrderId { get; }

        public CreateOrderResult(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
