namespace Ordering.Domain.Abstractions;

// make this class as an abstract class in order to as a base entity
// class and make these entity as a generic to inherit from IEntity.
public abstract class Entity<T> : IEntity<T>
{
    public T Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
}
// this is the abstract class and provide a base implementation of 
// the IEntity. 

// and it is a generic class that can be used to define entities 
// without various type of identifiers, including string, integer, 
// even the custom types. 