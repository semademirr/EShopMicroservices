

using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries.GetOrdersByName;

public class GetOrdersByNameHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
{
    public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
    {
        // get orders by name using dbContext
        // return result

        var orders = await dbContext.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking() // this is a good optimization when you are only reading a data and dont plan to update these entities in the same context. it tells ef core not to track changes to.
            .Where(o => o.OrderName.Value.Contains(query.Name))
            .OrderBy(o => o.OrderName) // this will be provide sorting the result by the order name
            .ToListAsync(cancellationToken);

        return new GetOrdersByNameResult(orders.ToOrderDtoList());
    }

    
}
