namespace PureBakes.Service.Services.Interface;

using PureBakes.Models;

public interface IProductService
{
    IEnumerable<Product> GetAll();
    Product Get(int productId);
    void Add(Product product);
    void Update(Product product);
}