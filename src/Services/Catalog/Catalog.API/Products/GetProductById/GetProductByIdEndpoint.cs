
namespace Catalog.API.Products.GetProductById;

// public record GetProductByIdRequest();
public record GetProductByIdResponse(Product Product);
public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id}", async (Guid id, ISender sender) =>
        {
            // now we will accodomate the incoming requests and paste 
            // the mediator. for that purpose we are gonna define a result
            // variable and sending with the mediator with using the sender.

            var result = await sender.Send(new GetProductByIdQuery(id));
            // in the query object we passing the id information which comes
            // from the our request parameters. 

            // after that we can define a response object with mapping the result
            // object: 
            var response = result.Adapt<GetProductByIdResponse>();

            // and lastly we will return to the client with this response object.
            return Results.Ok(response);
        })
            // and always we will configure these Http methods get 
            // endpoint with using the extension methods we will define
            // the api name summary description and produce the Http results
            // according to these implementation. 
            .WithName("GetProductById")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product By Id")
            .WithDescription("Get Product By Id");
        // basically in this endpoint definition we are mapping the 
        // Get request with this product/id information which takes 
        // a product id as a parameter. 

        // and the endpoint is using the mediator in order to send get 
        // product by id query object, then mediator will be trigger the 
        // corresponding query handler, and after that with getting the result
        // object mapping this result object to the or get product by id response 
        // object and return to the client as a result that okey200 response. 
    }

}
