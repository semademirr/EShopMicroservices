
namespace Catalog.API.Products.CreateProduct;

// we will define our request and response record types.

public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);
public record CreateProductResponse(Guid Id);
  
// create product endpoint class will be responsible for setting up
// our post endpoint. we will exposing post endpoint in here using the
// minimal api and Carter. 
public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // in this function we will define our Http post endpoint 
        // using Carter and Mapster. and after that we will map the 
        // our request to a command object. and after that we will send 
        // it through the mediator and then map the result back to
        // response model. 
        app.MapPost("/products",
            async (CreateProductRequest request, ISender sender) =>
        {
            // now we will convert using the Mapster from request to 
            // command object. 
            var command = request.Adapt<CreateProductCommand>();

            var result = await sender.Send(command);
            // this command will start the mediator and trigger the handler class.

            var response = result.Adapt<CreateProductResponse>();

            return Results.Created($"/products/{response.Id}", response);

        })
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Create Product");

        
    }
}

