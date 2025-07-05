

namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<Product> Products);
// these record types represent to request for feching products and 
// the structure of the data that we expect to return. 

// this class is responsible for handling the GetProducts query 
// and returning to the GetProduct result. 
// it is the best practice to separate query and commands in the
// names of the class, and this will be GetproductsQueryHandler
// 
internal class GetProductsQueryHandler(IDocumentSession session)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{

    // in this handler class we basically use IDocumentation from the 
    // Marten in order to query the products. 
    // even we dont use any query object change request to query. because
    // this is the get all products and we dont any filter operation in the query.
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await session.Query<Product>()
            .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);
        return new GetProductsResult(products);
    }
}
