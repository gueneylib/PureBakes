namespace PureBakes.Service.Services;

using PureBakes.Data.Repository.Interface;
using PureBakes.Models;
using PureBakes.Service.Services.Interface;

public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    public IEnumerable<Category> GetAll()
    {
        return categoryRepository.GetAll();
    }

    public Category Get(int categoryId)
    {
        return categoryRepository.Get(categoryId) ?? new Category();
    }

    public void Add(Category category)
    {
        categoryRepository.Add(category);
        categoryRepository.Save();
    }

    public void Update(Category category)
    {
        categoryRepository.Update(category);
        categoryRepository.Save();
    }
}