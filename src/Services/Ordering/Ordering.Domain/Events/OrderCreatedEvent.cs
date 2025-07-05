

namespace Ordering.Domain.Events;

public record OrderCreatedEvent(Order order) : IDomainEvent;
// the orderCreatedEvent also carrying the order data as a parameter
// of the domain event, and this can include all the details of the 
// order that was just created. 
