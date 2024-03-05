namespace PureBakes.Service.Services.Interface;

using PureBakes.Models;

public interface IShoppingCartService
{
    ShoppingCart GetShoppingCartByUserId(string userId);
    int GetShoppingCartProductsQuantity();
    void UpdateCartItem(ShoppingCartItem match);
    IEnumerable<ShoppingCartItem> GetAllProductsInCart();
}