namespace PureBakes.Service.Services.Interface;

using PureBakes.Models;

public interface IShoppingCartService
{
    int GetShoppingCartProductsQuantity();
    void UpdateCartItem(ShoppingCartItem shoppingCartItem);
    IEnumerable<ShoppingCartItem> GetAllProductsInCart();
    bool IncrementProductQuantity(int cartItemId);
    bool DecrementProductQuantity(int cartItemId);
    bool RemoveProductFromCart(int cartItemId);
}