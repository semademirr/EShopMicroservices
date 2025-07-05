
namespace Ordering.Domain.Abstractions;
// in the IAggregate interface we are gonna define domain 
// events as a IReadOnly list.

// IAggregate is a special kind of entity that can handle the domain events.

public interface IAggregate<T> : IAggregate, IEntity<T>
{
    // this will be basically define an interface of the IAggregate with
    // the generic type and inherit form the IAggregate interface that 
    // we defined in here and IEntity generic type T. 
}

public interface IAggregate : IEntity
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    IDomainEvent[] ClearDomainEvents();
}
