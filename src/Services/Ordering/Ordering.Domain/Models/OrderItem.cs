

namespace Ordering.Domain.Models;

public class OrderItem : Entity<OrderItemId>
{

    // this constructor is the internal constructor that we can only
    // reach access in the domain layer  
    internal OrderItem(OrderId orderId, ProductId productId, int quantity, decimal price)
    {
        Id = OrderItemId.Of(Guid.NewGuid());
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }

    public OrderId OrderId { get; private set; } = default!;
    public ProductId ProductId { get; private set; } = default!;
    public int Quantity { get; private set; } = default!;
    public decimal Price { get; private set; } = default!;
}
// order item represents individual items within an order, and it
// contains product information and quantity information and so on. 