
using MediatR;
// this will be the only one package for the ordering domain to 
// implement domain event pattern into the ordering layer. 

namespace Ordering.Domain.Abstractions;

// INotification is from the mediatR. mediatR provides very
// good interface to implement domain event operations inside 
// of the domain layers. 
public interface IDomainEvent : INotification
{
    Guid EventId => Guid.NewGuid();
    public DateTime OccuredOn => DateTime.Now;
    public string EventType => GetType().AssemblyQualifiedName;
    // which will be which class throwing this domain event understanding
    // from the assembly information into these interface. 
}

// as you can see that we have created a domain event which extends 
// from the INotification mediatr. 

// why we use INotification interface? Because it is allowing events
// to dispatched through the mediatr handlers. so by this way we can
// use mediatr handlers in order to handle these domain event using 
// the mediatr libraries. 