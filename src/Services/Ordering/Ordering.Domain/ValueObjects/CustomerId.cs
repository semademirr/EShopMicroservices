
namespace Ordering.Domain.ValueObjects;

public record CustomerId
{
    public Guid Value { get; }
    private CustomerId(Guid value) => Value = value;
    public static CustomerId Of(Guid value) // for creating customer id instances
    {
        ArgumentNullException.ThrowIfNull(value);
        if(value == Guid.Empty)
        {
            throw new DomainException("CustomerId cannot be empty.");
        }
        return new CustomerId(value);
    }

}
// we can define the customer id record type. this is the value
// object and these will be the are strongly type id, including
// the value primitive object inside of the customer id concrete
// type. 