
namespace Catalog.API.Products.GetProducts;
// in this class we will develop our request and response object since
// we are developing an Http get endpoint, our request doesnt require
// a Json input. However we will define a response record structure 
// in order to return with data. So that purpose i am coming here and 
// i am gonna put a command at request object to make it best practice
// we should always define request and response in the endpoint and 
// always define query or command and result in the handler class. 

// we dont required any requet object but we will see in here.
// public record GetProductsRequest();


// this is our current GetProductRequest object. that will get these 
// information from the request. 
// we will get this request object as a request parameter into the 
// our application endpoint. 
public record GetProductRequest(int? PageNumber = 1, int? PageSize = 10);

// it is getting a parameter which name is enumerable product called Products.
public record GetProductResponse(IEnumerable<Product> Products);
// if you go to the get products handler class the result should be 
// the same structure and see that this is getting exactly same parameter 
// and parameter name is the same. By this way mapster can be adapted 
// without any further configuration. 

// we will use Carter to create our GetEndpoint class. 
// Carter simplifies the definition and configuration of routes in asp.net core

public class GetProductsEndpoint : ICarterModule
{
    // in this method we will exposing http get method and define 

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // we will implement actual logic with using the app.mapGet method.
        app.MapGet("/products", async ([AsParameters] GetProductRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetProductsQuery>();
            // we can se that this is converting from the incoming request
            // to the get products query. 

            var result = await sender.Send(query);
            // after getting response from the mediator we can convert 
            // our response type from the mediator handler.

            var response = result.Adapt<GetProductResponse>();
            // to use mapster in that way that mean is we can directly 
            // pass result object to do our response object. 

            return Results.Ok(response);

            // after define any http method with carter, we can define
            // additional configurations: 
        })
            .WithName("GetProducts")
            .Produces<GetProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products")
            .WithDescription("Get Products");
    }
}

// if you go on top of this page you can see that there is no any using 
// statement because all using statement related classes will already
// including our global using. so we dont need any using statement
// in this class and it makes it more cleaner. 