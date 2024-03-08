namespace PureBakes.Data.Repository;

using PureBakes.Data.Repository.Interface;
using PureBakes.Models;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly PureBakesDbContext _dbContext;

    public CategoryRepository(PureBakesDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public void Update(Category category)
    {
        _dbContext.Update(category);
    }
}