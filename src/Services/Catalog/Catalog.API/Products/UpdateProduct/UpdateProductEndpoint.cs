
namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductRequest(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price);
// these parameter names should be exactly same with the command 
// object int the update product handler. Because this mapster will be 
// mapped these objects looking for the parameter type and parameter name.
public record UpdateProductResponse(bool IsSuccess);

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products",
            async (UpdateProductRequest request, ISender sender) =>
            {
                // in this function we are gonna implement acoodomating
                // request with getting the command value from the request object.
                // we are gonna adapting incoming requests and converting
                // these requests to the update product command. 
                // because the mediator is requiring command and query.
                // that is why we should define a command object in here
                // with mapping from the incoming request object. 
                var command = request.Adapt<UpdateProductCommand>();

                // the second thing we will perform getting result information
                // from the mediator with sending using sender.send method.
                var result = await sender.Send(command);
                // after sending the command object, mediator will be trigger 
                // command handler and return back to the result.

                // and after that again we are gonna use mapster to converting 
                // result object to the response object.
                var response = result.Adapt<UpdateProductResponse>();

                return Results.Ok(response);
            })
            .WithName("UpdateProduct")
            .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Product")
            .WithDescription("Update Product");
            
    }
}
// in this endpoint class we basically define a put request for 
// /products endpoint and the endpoint recieve an update product request
// object, and it adapt it to do an update product command using the 
// mapster. and we basically send this command through the mediator.
// and mediator will be trigger the command handler and respose back the result object.

// in postman for update request we will additionally add id information
// into the updated product details.