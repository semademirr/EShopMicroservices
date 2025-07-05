
namespace Ordering.Application.Orders.Commands.UpdateOrder;

public class UpdateOrderHandler(IApplicationDbContext dbContext)
    : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
    public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        // update order entity from command object
        // save to database
        // return result

        var orderId = OrderId.Of(command.Order.Id);
        var order = await dbContext.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
            //.FindAsync([orderId], cancellationToken: cancellationToken);

        if (order is null)
        {
            throw new OrderNotFoundException(command.Order.Id);
        }

        UpdateOrderWithNewValues(order, command.Order);

        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateOrderResult(true); // which mean is this update operation is performed succesfully.
    }


    public void UpdateOrderWithNewValues(Order order, OrderDto orderDto)
    {
        // we are gonna map the values from dto to order entity.
        var updatedShippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);
        var updatedbillingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName, orderDto.BillingAddress.EmailAddress, orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.Country, orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);
        var updatedPayment = Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.Expiration, orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod);

        order.Update(
            orderName: OrderName.Of(orderDto.OrderName),
            shippingAddress: updatedShippingAddress,
            billingAddress: updatedbillingAddress,
            payment: updatedPayment,
            status: orderDto.Status);

        while (order.OrderItems.Any())
        {
            // Remove the first item repeatedly until empty.
            var firstItem = order.OrderItems.First();
            order.Remove(firstItem.ProductId);
        }

        // Add new items from the payload.
        foreach (var dtoItem in orderDto.OrderItems)
        {
            order.Add(
                productId: ProductId.Of(dtoItem.ProductId),
                quantity: dtoItem.Quantity,
                price: dtoItem.Price);

        }
    }
}
