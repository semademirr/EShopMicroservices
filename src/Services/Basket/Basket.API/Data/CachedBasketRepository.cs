
// in this class we basically inherit from the IBasketRepository 
// and encapsulate an instance of the basket repository, and we will
// override the methods to add caching logic. 

// basically we have implemented two patterns proxy pattern and decorator
// pattern. For proxy pattern: CachedBasketRepository acts as a proxy
// and forwarding the calls to underlying basket repository. 

// and also we will implement decorator pattern. That mean is we will
// extend the functionality of basketrepository by adding caching logic.

// we will implementing caching logic within each method and reduce
// the database calls. so we will leverage the redis connection through
// IDistributedCache interface provided by .net. 

// basically before calling the database operation we are gonna create
// a cache basket variable and retrieve this basket information try
// to get from distributed cache. 

using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
// we are not moving these using statement to gloabla using because 
// these are only using in the cachedbasketrepository that would not 
// be using the other classes.

namespace Basket.API.Data;

public class CachedBasketRepository
    (IBasketRepository repository, IDistributedCache cache) 
    : IBasketRepository
{
    
    public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
        // within this code, we basically call getStringAsync method
        // with passing the username as a key information. 

        if(!string.IsNullOrEmpty(cachedBasket))
            // if not null we can perform the Json deserialize operation.
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket);

        var basket = await repository.GetBasket(userName, cancellationToken);

        await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);

        return basket;
        
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        await repository.StoreBasket(basket,cancellationToken);

        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);
    
        return basket;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
        await repository.DeleteBasket(userName,cancellationToken);

        await cache.RemoveAsync(userName, cancellationToken);
        
        return true;
    }
}
