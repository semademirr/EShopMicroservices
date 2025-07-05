
using Discount.Grpc;

namespace Basket.API.Basket.StoreBasket;

// this cart variable name is crucial when sending the request to 
// the http post operation. If you change the name as a cat two or 
// shopping cart and if you go back to http post operation your json
// would not be work and your json is expecting the exact same name 
// parameter into in here. 
public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    // we are gonna develop this validator and validations always 
    // perform in the constructor.

    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
        RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
    }
    // withing this code we basically validate the commands using 
    // the fluent validation abstract validator class. and this will
    // be comes from the building blocks. 

}

public class StoreBasketCommandHandler
    (IBasketRepository repository, DiscountProtoService.DiscountProtoServiceClient discountProto)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        // we can safely call deduct method.
        await DeductDiscount(command.Cart, cancellationToken);

        // TODO: store basket in database (use marten upsert - if exist update, if not insert any record)
        await repository.StoreBasket(command.Cart, cancellationToken);

        return new StoreBasketResult(command.Cart.UserName);
    }

    private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
    {
        // this is the method that we have extracting this todo
        // statement and operation. 

        // comunnicate with discount.grpc and calculate lastest prices of 
        // products into cart.
        foreach(var item in cart.Items)
        {
            var coupon = await discountProto.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
            item.Price -= coupon.Amount;
        }
    }
}
