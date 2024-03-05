namespace PureBakes.Service.Services.Interface;

using PureBakes.Models;

public interface IProductService
{
    IEnumerable<Product> GetAll();
    Product? Get(int productId);
    void Add(Product product);
    void Update(Product product);
    bool Remove(int productId);
    string GetImageUrlOfProduct(int productId);
}