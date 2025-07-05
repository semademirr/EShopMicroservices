
namespace Basket.API.Exceptions;

public class BasketNotFoundException : NotFoundException
{
    // this will be provide a clear and meaningful exception when
    // a basket is not found in the database.
    public BasketNotFoundException(string userName) : base("Basket", userName)
    {
        
    }
}
