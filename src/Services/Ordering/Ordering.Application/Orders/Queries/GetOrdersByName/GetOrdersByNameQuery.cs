
namespace Ordering.Application.Orders.Queries.GetOrdersByName;

public record GetOrdersByNameQuery(string Name) 
    : IQuery<GetOrdersByNameResult>;

public record GetOrdersByNameResult(IEnumerable<OrderDto> Orders);

// this class represents the query in order to retrieve orders 
// by a given name, and it contains a single string name property. 
