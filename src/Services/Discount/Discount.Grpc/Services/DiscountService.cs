using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace Discount.Grpc.Services;

public class DiscountService
    (DiscountContext dbContext, ILogger<DiscountContext> logger)
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
         var coupon = await dbContext
            .Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

        if (coupon is null)
            coupon = new Coupon { ProductName = "No Discount", Amount = 0 , Description = "No Discount Desc"};

        // we have to convert this coupon object as a coupon model

        logger.LogInformation("Discount is retrieved for ProductName:  {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);


        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
        
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        // the first thing we should do that we should get the 
        // incoming request object and reach the coupon object 
        // from converting the our coupon to the Request Coupon 
        // object.
        var coupon = request.Coupon.Adapt<Coupon>();

        // if the coupon is null, we basically throw an exception
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

        // after that, if the coupon is not null, we can perform the 
        // database insert operation for the create discount method.
        dbContext.Coupons.Add(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount is succesfully created. ProductName : {productName}", coupon.ProductName);


        // lastly we will return back to our client with the CouponModel object.
        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public async override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

        
        dbContext.Coupons.Update(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount is succesfully updated. ProductName : {productName}", coupon.ProductName);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        // the first thing we should do that we are going to retrieve
        // the coupon information from the database.
        var coupon = await dbContext
            .Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

        if (coupon is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));

        dbContext.Coupons .Remove(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount is successfully deleted. ProductName: {ProductNmae}", request.ProductName);

        // lastly we will return back to the rpc client as a return new 
        // delete discount response and passing the succes property as true.
        return new DeleteDiscountResponse { Success = true };
    }

}
