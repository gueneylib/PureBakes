namespace PureBakes.Data.Repository;

using PureBakes.Data.Repository.Interface;

public class UnitOfWork : IUnitOfWork
{
    private readonly PureBakesDbContext _dbContext;
    public ICategoryRepository Category { get; }
    public IProductRepository Product { get; }

    public UnitOfWork(PureBakesDbContext dbContext)
    {
        _dbContext = dbContext;
        Category = new CategoryRepository(_dbContext);
        Product = new ProductRepository(_dbContext);
    }
    public void Save()
    {
        _dbContext.SaveChanges();
    }
}