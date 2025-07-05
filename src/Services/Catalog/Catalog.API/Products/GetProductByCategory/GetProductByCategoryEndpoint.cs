
namespace Catalog.API.Products.GetProductByCategory;

// we are gonna develop get product by category endpoint class with 
// using minimal apis and carter. we will start with defining request 
// and response object. and we dont have any request object because
// we will get the category information from the request parameter. 

// public record GetProductByCategoryRequest();

// after that we will create a response object which will be the 
// including IEnumerable product object. this will be our request and response object.
public record GetProductByCategoryResponse(IEnumerable<Product> Products);

// now we can implement actual endpoint class and this will be implement
// ICarter model. and this class will define our Get endpoint for 
// fetching products by a specific category. 
public class GetProductByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // we have to put additional category in here because we have
        // already using the id request parameter under the products.
        // so in order to get category information we have to define additional layer here.
        // that's why we have added products category and category name
        // getting from the request parameter. to implement this handler 
        // method we can use asynchronous function. 
        app.MapGet("/products/category/{category}",
            async (string category, ISender sender) =>
        {
            // and we will implement asynchronous function in order to
            // accomodate these requests. 

            // and we will passing the category name this comes from the request parameters.
            // and this mediator will be returned a result object.
            var result = await sender.Send(new GetProductByCategoryQuery(category));

            // after that we basically adapting result object to our 
            // response object. 
            var response = result.Adapt<GetProductByCategoryResponse>();
            return Results.Ok(response);

        })
            .WithName("GetProductByCategory")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product By Category")
            .WithDescription("Get Product By Category");
    }
}
// in this endpoint definition we basically map Http get method with 
// these url products category and category name. and this endpoint
// uses mediator to send the get products by category query. 
// and then this will trigger the handler method and then adapts 
// to our response object that will be returned to the client. 