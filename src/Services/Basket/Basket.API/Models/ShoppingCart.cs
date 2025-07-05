namespace Basket.API.Models;
// this will be include a collection of the shopping cart item object.
public class ShoppingCart
{
    public string UserName { get; set; } = default;
    public List<ShoppingCartItem> Items { get; set; } = new();
    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);
    public ShoppingCart(string userName)
    {
        UserName = userName;
    }

     // Required for mapping 
     public ShoppingCart()
     {
     }
}
// in the shopping cart we dont have any ID property in here. 
// instead of that, we use username to identify for shopping cart object.
// in order to use username as a identifier in the document database
// we need to mark these username as a identity of shopping cart 
// document table. 