namespace PureBakes.Data.Repository;

using PureBakes.Data.Repository.Interface;


// Todo this unitofwork should be removed. I would like to separate the repositories and inject only one repo in one corresponding service
public class UnitOfWork : IUnitOfWork
{
    private readonly PureBakesDbContext _dbContext;
    public ICategoryRepository Category { get; }
    public IProductRepository Product { get; }
    public IShoppingCartRepository ShoppingCart { get; }
    public IShoppingCartItemRepository ShoppingCartItem { get; set; }

    public UnitOfWork(PureBakesDbContext dbContext)
    {
        _dbContext = dbContext;
        Category = new CategoryRepository(_dbContext);
        Product = new ProductRepository(_dbContext);
        ShoppingCart = new ShoppingCartRepository(_dbContext);
        ShoppingCartItem = new ShoppingCartItemRepository(_dbContext);
    }
    public void Save()
    {
        _dbContext.SaveChanges();
    }
}