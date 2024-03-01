namespace PureBakes.Service.Services.Interface;

using PureBakes.Models;

public interface IShoppingCartService
{
    ShoppingCart GetShoppingCartByUserId(string userId);
    void CreateShoppingCartForUserIfNecessary(string userId);
}