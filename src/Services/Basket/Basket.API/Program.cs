
using BuildingBlocks.Exceptions.Handler;
using Discount.Grpc;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Caching.Distributed;
using BuildingBlocks.Messaging.MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// application services 
var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// data services 

// so within this code, we basically configure Marten with our 
// PostgreSQL connection string. And we specify the shopping cart
// entity will use the username property as its identity field. 
// and lastly we will use lightweight sessions to optimize performance
// by utilizing Marten's lightweight session mode. 
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);

    // in here we are gonna configure our Marten options again using the 
    // options. 
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    // options.InstanceName = "Basket";
});


// grpc services
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
    return handler;
});

// Async Communication Services
builder.Services.AddMessageBroker(builder.Configuration); // publisher side dont need to assembly.

// cross cutting services
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

// first is we will register health checks before building the application.
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

var app = builder.Build();


// and after that we will configure the middleware. 


// Configure the HTTP request pipeline. 
app.MapCarter();
// this will be map the carter endpoints into our asp.net web api project.

app.UseExceptionHandler(options => { });

// this will be make our health endpoint response as a Json response
// including all entries. 
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
app.Run();
