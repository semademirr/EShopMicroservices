
namespace Ordering.Application.Dtos;

// this dto represents the essential information of each item in 
// an order such as product id, quantity etc. 
public record OrderItemDto(Guid OrderId, Guid ProductId, int Quantity, decimal Price);