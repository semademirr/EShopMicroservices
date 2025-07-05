
namespace Catalog.API.Products.GetProductByCategory;

// in this handle method this will be responsible for handling the 
// query for fetching products by category. 
public record GetProductByCategoryQuery(string Category)
    : IQuery<GetProductByCategoryResult>;

public record GetProductByCategoryResult(IEnumerable<Product> Products);

internal class GetProductByCategoryQueryHandler
    (IDocumentSession session)
    : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{

    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        
        // this method handle the asynchronously fetch all matching 
        // products with given category information and returns them 
        // as a part of the our result object in a IEnumerable product 
        // object. 
        var products = await session.Query<Product>()
            .Where(p => p.Category.Contains(query.Category))
            .ToListAsync();
        return new GetProductByCategoryResult(products);
    }
}
// in this handle method we use IDocumentSession from the Marten in 
// order to query this product document table by category. 