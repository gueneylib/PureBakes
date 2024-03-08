namespace PureBakes.Data.Repository.Interface;

using PureBakes.Models;

public interface IShoppingCartItemRepository : IRepository<ShoppingCartItem>
{
    new ShoppingCartItem? Get(object id, string includeProperties = "", bool tracked = false);

    void Update(ShoppingCartItem shoppingCartItem);
}