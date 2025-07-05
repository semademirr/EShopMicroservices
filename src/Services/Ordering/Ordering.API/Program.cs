using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure.Data.Extensions;
using Ordering.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. This representing asp.net 
// built-in dependecy injection container.
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructreServices(builder.Configuration)
    .AddApiServices(builder.Configuration);
// we are accumulating all these registrations according to clean
// architecture layers. that mean is before adding infrastructure
// services we should register the mediator into application 
// service leyers. 

var app = builder.Build();

// Configure the Http request pipeline. 
app.UseApiServices();

if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync(); // first step.
}

app.Run();
