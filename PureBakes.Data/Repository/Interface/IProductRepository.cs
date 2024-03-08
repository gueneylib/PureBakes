namespace PureBakes.Data.Repository.Interface;

using PureBakes.Models;

public interface IProductRepository : IRepository<Product>
{
    void Update(Product product);
}