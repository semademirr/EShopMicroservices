
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCarter();
        services.AddExceptionHandler<CustomExceptionHandler>(); // with this line of code we will inject our custom exception handler into the service collection, which will be used to handle exceptions globally in the application.
        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("Database"));
        return services;    
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        app.MapCarter(); // which is configured the application to use Carter for request handling effectively mapping to define minimal api endpoints. 
        app.UseExceptionHandler(options => { }); // this line of code is used to configure the exception handler middleware, which will handle exceptions globally in the application.
        app.UseHealthChecks("/health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            });
        return app;
    }
}
