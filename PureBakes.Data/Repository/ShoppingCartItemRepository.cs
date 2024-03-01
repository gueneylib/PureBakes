namespace PureBakes.Data.Repository;

using System.Xml.Linq;
using PureBakes.Data.Repository.Interface;
using PureBakes.Models;

public class ShoppingCartItemRepository : Repository<ShoppingCartItem>, IShoppingCartItemRepository
{
    private readonly PureBakesDbContext _dbContext;

    public ShoppingCartItemRepository(PureBakesDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public void Update(ShoppingCartItem shoppingCartItem)
    {
        var shoppingCartItemFromDb = _dbContext.ShoppingCartItems.FirstOrDefault(u => u.Id == shoppingCartItem.Id);
        if (shoppingCartItemFromDb != null)
        {
            shoppingCartItemFromDb.ShoppingCartId = shoppingCartItem.ShoppingCartId;
            shoppingCartItemFromDb.ProductId = shoppingCartItem.ProductId;
            shoppingCartItemFromDb.Quantity = shoppingCartItem.Quantity;
        }
        else
        {
            var newItem = new ShoppingCartItem
            {
                // Product = shoppingCartItem.Product,
                ProductId = shoppingCartItem.ProductId,
                Quantity = shoppingCartItem.Quantity,
                // ShoppingCart = shoppingCartItem.ShoppingCart,
                ShoppingCartId = shoppingCartItem.ShoppingCartId
            };
            Add(newItem);
        }
    }
}