using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Data;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    // in this method we also would like to access Iconfiguration 
    // in order to access the connection strings.
    public static IServiceCollection AddInfrastructreServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");

        // add services to the container
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>()); 
            options.UseSqlServer(connectionString);
        });

        // this registration enables of IApplicationDbContext interface
        // throught the application layer, where we implement actual 
        // business logic, which is the create order handler.
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        
        return services;
    }
}
