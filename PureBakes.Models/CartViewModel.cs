namespace PureBakes.Models;

public class CartViewModel
{
    public IEnumerable<ShoppingCartItem> ShoppingCartItems { get; set; } = Array.Empty<ShoppingCartItem>();
    public double TotalCartPrice { get; set; }
}