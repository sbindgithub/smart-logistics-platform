namespace SmartLogistics.API.Models
{
    /// <summary>
    /// API request model for creating a new order.
    /// This model represents the external contract exposed by the API.
    /// </summary>
    public class CreateOrderRequest
    {
        /// <summary>
        /// Identifier of the customer placing the order.
        /// </summary>
        public Guid CustomerId { get; set; }
    }
}
