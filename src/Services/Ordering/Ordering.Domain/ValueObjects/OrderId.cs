
namespace Ordering.Domain.ValueObjects;

public record OrderId
{
    public Guid Value { get; }

    // private constructor.
    private OrderId(Guid value) => Value = value;

    public static OrderId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if(value == null)
        {
            throw new DomainException("OrderId cannot be empty.");
        }
        return new OrderId(value);
    }
}
