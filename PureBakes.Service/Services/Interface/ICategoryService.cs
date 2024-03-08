namespace PureBakes.Service.Services.Interface;

using PureBakes.Models;

public interface ICategoryService
{
    IEnumerable<Category> GetAll();
    Category Get(int productId);
    void Add(Category product);
    void Update(Category product);
}