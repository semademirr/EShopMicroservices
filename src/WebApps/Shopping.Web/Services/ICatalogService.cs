
namespace Shopping.Web.Services;

// this will be communicate yarp api gateway using the refit library.
public interface ICatalogService
{
    [Get("/catalog-service/products?pageNumber={pageNumber}&pageSize={pageSize}")]  
    Task<GetProductsResponse> GetProducts(int? pageNumber = 1, int? pageSize = 10);
    
    [Get("/catalog-service/products/{id}")]
    Task<GetProductByIdResponse> GetProduct(Guid id);
    
    [Get("/catalog-service/products/category/{category}")]
    Task<GetProductByCategoryResponse> GetProductByCategory(string category);
}

// we will use refit for consuming yarp api gateway endpoints