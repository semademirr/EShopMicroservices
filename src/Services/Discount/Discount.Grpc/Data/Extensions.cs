using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

// in this extension method we basically make this class static
// order to create an extension method for the IApplication builder.

public static class Extensions
{
    // we will create use migration extension method for automating
    // database migrate operations. 

    // in this method we basically create a scope in the beggining of 
    // method. so this scope will be help to us getting our db context 
    // object. 
    public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
    {
        // the scope is creating from the application builder, and after 
        // that we can reach the db context object. 
        using var scope = app.ApplicationServices.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DiscountContext>();
        dbContext.Database.MigrateAsync();

        return app;
    }
}

// after developing this extension method, we can register and use these
// migration extension method into program.cs. 