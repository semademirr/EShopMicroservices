
namespace Basket.API.Basket.StoreBasket;

public record StoreBasketRequest(ShoppingCart Cart);
public record StoreBasketResponse(string UserName);
// this parameter types and names should be same with handler class.

public class StoreBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (StoreBasketRequest request, ISender sender) =>
        {
            // the first line of code in the delegate handler we 
            // basically adapting the incoming request to our command object.
            var command = request.Adapt<StoreBasketCommand>();

            // after that we can send this command object to the mediator.
            var result = await sender.Send(command);
            // this will be run the command handler method and return back to
            // us result. 

            var response = result.Adapt<StoreBasketResponse>();
            return Results.Created($"/basket/{response.UserName}", response);
            // created response which is returning 201 http status code.
        })
            .WithName("CreateProduct")
            .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Create Product");
    }
}
