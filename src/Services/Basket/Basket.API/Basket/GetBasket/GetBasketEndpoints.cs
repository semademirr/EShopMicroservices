
namespace Basket.API.Basket.GetBasket;
// representing our api definitions. 

// public record GetBasketRequest(string UserName);

public record GetBasketResponse(ShoppingCart Cart);
public class GetBasketEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // so in delegate handler method the first parameter is
        // populated from the request parameters 
        // and the second parameter will be injected by asp.net built in 
        // dependency injection. 
        app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
        {
            var result = await sender.Send(new GetBasketQuery(userName));

            // after that we can convert this result object as a response
            var response = result.Adapt<GetBasketResponse>();

            // we can return back to the client application this response object
            return Results.Ok(response);
        })
            .WithName("GetProductById")
            .Produces<GetBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product By Id")
            .WithDescription("Get Product By Id");
    }
}
