
using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.API.Products.DeleteProduct;

// as always we will start with the defining record types. In the 
// endpoint class, we have defined request and response record types.
// and after that we will perform and exposing Http delete operation 
// with using Carter. 

// we basically require a guid id information that we can get from the 
// request parameters. 
// public record DeleteProductRequest(Guid Id);

public record DeleteProductResponse(bool IsSuccess);
// IsSucces property should be same as the our result object into 
// the handler class in order to convert each other easily using the mapster.

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteProductCommand(id));
            // in the product command we are gonna pass the id information
            // that comes from the request parameters. and mediator will 
            // be triggered the delete product command handler class and 
            // return back the result. 

            var response = result.Adapt<DeleteProductResponse>();

            return Results.Ok(response);
        })
           .WithName("DeleteProduct")
           .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Delete Product")
           .WithDescription("Delete Product");


    }
}
