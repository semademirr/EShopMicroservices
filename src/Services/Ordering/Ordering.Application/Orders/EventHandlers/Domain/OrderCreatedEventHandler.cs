using MassTransit;
using Microsoft.FeatureManagement;

namespace Ordering.Application.Orders.EventHandlers.Domain;

public class OrderCreatedEventHandler
    (IPublishEndpoint publishEndpoint, IFeatureManager featureManager, ILogger<OrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedEvent> // this will be trigger when mediator publishing the notification from the infrastructure layer. 
{
    public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

        if (await featureManager.IsEnabledAsync("OrderFullfilment")) // orderfullfilment exactly same our appsettings.json file in the ordering.api layer.
        {
            // publishing integration event part
            var orderCreatedIntegrationEvent = domainEvent.order.ToOrderDto();
            await publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);
        }

    }
}
