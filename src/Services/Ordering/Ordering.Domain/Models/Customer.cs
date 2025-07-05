
namespace Ordering.Domain.Models;

public class Customer : Entity<CustomerId>
{
    public string Name { get; private set; } = default!;
    public string Email { get; private set; } = default!;

    public static Customer Create(CustomerId id, string name, string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        var customer = new Customer
        {
            Id = id,
            Name = name,
            Email = email
        };

        return customer;
    }

    public static Customer Create(object custo, object value, string v1, string v2)
    {
        throw new NotImplementedException();
    }
}
