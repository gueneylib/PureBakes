namespace PureBakes.Data.Repository;

using PureBakes.Data.Repository.Interface;
using PureBakes.Models;

public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
{
    private readonly PureBakesDbContext _dbContext;
    public ShoppingCartRepository(PureBakesDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}