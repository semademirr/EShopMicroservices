

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// this will be registered carter classes into asp .net dependency injection container.
// this is adding the necessary services for Carter into asp.netcore

var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config =>
{
    // tells the mediator where to find our command and query 
    // handler classes. 
    config.RegisterServicesFromAssembly(assembly);
    // this is adding required services into mediator.
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    // in here we basically add our validation behavior as a pipeline
    // behavior into mediatr. we also register all validators from our 
    // assembly with this way. by using this pipeline behavior we achieve
    // the cleaner handler methods focused on the business logic. 
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// this method comes from the FluentValidation dependency injection 
// extension that will facilitate to inject validation for our asp.net
// applications. 
builder.Services.AddValidatorsFromAssembly(assembly);


builder.Services.AddCarter();

// Marten Configuration
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database"));
}). UseLightweightSessions();


if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();
// this will be provided to our seeding operation when the application
// start up.

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

// this line of code will be performed the health check for the PostgreSQL
// database withing the catalog microservices. 
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database"));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();
// Carter maps the routes defined in the ICarter module implementation.

// we will inject actual custom exception handler class which is a more
// modular approach using a custom exception handler. 
// in order to utilize our custom exception handler, we need to 
// register it in our application services and configure it in the
// request pipelines. 
app.UseExceptionHandler(options => { });
// the empty option parameter indicates that we are relying
// on custom configured handler. 

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
