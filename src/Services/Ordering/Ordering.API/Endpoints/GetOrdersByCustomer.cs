﻿
using Microsoft.AspNetCore.Http.HttpResults;
using Ordering.Application.Orders.Queries.GetOrdersByCustomer;

namespace Ordering.API.Endpoints;

// - Accepting a customer ID.
// - Uses a GetOrdersByCustomerQuery to fetch orders
// - Returns the list of orders for that customer

//public record GetOrdersByCustomerRequest(Guid CustomerId);
public record GetOrdersByCustomerResponse(IEnumerable<OrderDto> Orders);
public class GetOrdersByCustomer : ICarterModule
{
    public async void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/customer/{customerId}", async (Guid customerId, ISender sender) =>
        {
            var result = sender.Send(new GetOrdersByCustomerQuery(customerId));
            var response = result.Adapt<GetOrdersByCustomerResponse>();
            return Results.Ok(response);
        })
            .WithName("GetOrdersByCustomer")
            .Produces<GetOrdersByCustomerResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Orders By Customer")
            .WithDescription("Get Orders By Customer");
    }
}
