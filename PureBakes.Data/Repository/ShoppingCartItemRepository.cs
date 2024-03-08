namespace PureBakes.Data.Repository;

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
        _dbContext.Update(shoppingCartItem);
    }
}