namespace PureBakes.Data.Repository;

using PureBakes.Data.Repository.Interface;
using PureBakes.Models;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly PureBakesDbContext _dbContext;

    public ProductRepository(PureBakesDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    public void Update(Product product)
    {
        _dbContext.Update(product);
    }
}