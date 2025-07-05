
using System.Net;

namespace Shopping.Web.Services;

public interface IBasketService // we will consume this methods from basket microservices. over the yarp api gateway
{
    [Get("/basket-service/basket/{userName}")]
    Task<GetBasketResponse> GetBasket(string userName);

    [Post("/basket-service/basket")]
    Task<StoreBasketResponse> StoreBasket(StoreBasketRequest request); // "request" must be same in services endpoints definition

    [Delete("/basket-service/basket/{userName}")]
    Task<DeleteBasketResponse> DeleteBasket(string userName);

    [Post("/basket-service/basket/checkout")]
    Task<CheckoutBasketResponse> CheckoutBasket(CheckoutBasketRequest request);

    public async Task<ShoppingCartModel> LoadUserBasket()
    {
        // Get basket if not exist create new basket with default logged in user name
        var userName = "swn";
        ShoppingCartModel basket;

        try
        {
            var getBasketResponse = await GetBasket(userName);
            basket = getBasketResponse.Cart;
        }
        catch (ApiException apiException) when (apiException.StatusCode == HttpStatusCode.NotFound)
        {
            // If basket not found, create a new one
            basket = new ShoppingCartModel
            {
                UserName = userName,
                Items = []
            };
        }

        return basket;
    }
}
// refit cannot map the object if the names will not match with the endpoint method parameters. 