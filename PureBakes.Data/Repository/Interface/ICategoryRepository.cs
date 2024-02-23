namespace PureBakes.Data.Repository.Interface;

using PureBakes.Models;

public interface ICategoryRepository : IRepository<Category>
{
    void Update(Category category);
}