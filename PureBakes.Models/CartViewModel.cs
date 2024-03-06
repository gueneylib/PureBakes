namespace PureBakes.Models;

public class CartViewModel
{
    public IEnumerable<ShoppingCartItem> ShoppingCartItems { get; set; }
    public double TotalCartPrice { get; set; }
}