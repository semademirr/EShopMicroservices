
using BuildingBlocks.Behaviors;
using BuildingBlocks.Messaging.MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using System.Reflection;

namespace Ordering.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>)); 
            // this setup ensures that every request passing through
            // the mediator is automatically validated and logged. 
        });

        services.AddFeatureManagement(); // we will enabling the feature managemenet for the ordering microservices 

        // register message broker. assembly parameter is necessary for consumer.
        services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());

        return services;
    }
}
