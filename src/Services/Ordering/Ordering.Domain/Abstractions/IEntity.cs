
namespace Ordering.Domain.Abstractions;

// which is defined a generic interface for all entities with a unique
// identifier of the type of the T, so that we will use for implementing
// the entity classes. 
public interface IEntity<T> : IEntity
{
    // inside of this IEntity generic type, we basically create
    // one property, which name is id and id type is a t generic. 
    public T Id { get; set; }
}
public interface IEntity
{
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set;}
}

// we are gonna use as a common property for IEntity interface.