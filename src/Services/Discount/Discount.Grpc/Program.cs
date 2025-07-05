
using Discount.Grpc.Data;
using Discount.Grpc.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddDbContext<DiscountContext>(opts =>
        opts.UseSqlite(builder.Configuration.GetConnectionString("Database")));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}
// Configure the HTTP request pipeline.
app.UseMigration();
// placing this extension method at the start of the Http request pipeline,
// ensure that the database migration occurs before application start 
// up processing and before accepting any requests. 

app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
