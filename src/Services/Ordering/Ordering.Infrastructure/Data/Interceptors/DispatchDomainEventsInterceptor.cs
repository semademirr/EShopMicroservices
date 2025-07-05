
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ordering.Infrastructure.Data.Interceptors;

public class DispatchDomainEventsInterceptor(IMediator mediator)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();   
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        await DispatchDomainEvents(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public async Task DispatchDomainEvents(DbContext? context)
    {
        if (context == null) return;

        // we are looking for db context entities which implemented
        // the IAggregate interface. 
        // we are looking aggregates that including any domain events
        // and we are adding these aggregates into this variable. 
        var aggregates = context.ChangeTracker
            .Entries<IAggregate>()
            .Where(a => a.Entity.DomainEvents.Any())
            .Select(a => a.Entity);

        // after that we can retrieve the domain events from this aggregate.
        var domainEvents = aggregates
            .SelectMany(a => a.DomainEvents)
            .ToList();

        // and once we get the domain events, we can dispatch these
        // events with using the mediatR. 

        aggregates.ToList().ForEach(a => a.ClearDomainEvents());
        // we are gonna iterate the aggregate domain events and clear 
        // all domain events into each entities and aggregates, because
        // we will dipatching domain events and we would like avoiding 
        // duplicate dispatch operations. 

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);

    }
}
