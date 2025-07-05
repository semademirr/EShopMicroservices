// this class implements ICommandHandler interface for handling
// the create order command. 



namespace Ordering.Application.Orders.Commands.CreateOrder;
// the handler's responsibility is to process the command
// and interact with the domain model and databases. 
public class CreateOrderHandler(IApplicationDbContext dbContext)
    : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{

    // this handler method will be trigerred from the mediator
    // handle method also act as a mediator between the incoming
    // command and the domain model.
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        // Create Order entity from command object
        // save to database
        // return result

        var order = CreateNewOrder(command.Order);

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateOrderResult(order.Id.Value);
    }

    private Order CreateNewOrder(OrderDto orderDto)
    {
        // in this method we will basically construct a new order
        // entity from provided the orderDto object.
        // and it will include the conversion of DTO properties to 
        // the domain object like the address, payment and order items.

        var shippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);
        var billingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName, orderDto.BillingAddress.EmailAddress, orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.Country, orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);

        var newOrder = Order.Create(
            id: OrderId.Of(Guid.NewGuid()),
            customerId: CustomerId.Of(orderDto.CustomerId),
            orderName: OrderName.Of(orderDto.OrderName),
            shippingAddress: shippingAddress,
            billingAddress: billingAddress,
            payment: Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.Expiration, orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod)
            );

        foreach(var orderItemDto in orderDto.OrderItems)
        {
            newOrder.Add(ProductId.Of(orderItemDto.ProductId), orderItemDto.Quantity, orderItemDto.Price);
        }

        return newOrder;
    }
}
