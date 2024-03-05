namespace PureBakes.Service.Services;

using PureBakes.Data.Repository.Interface;
using PureBakes.Models;
using PureBakes.Service.Services.Interface;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public IEnumerable<Product> GetAll()
    {
        return productRepository.GetAll(includeProperties: "Category");
    }

    public Product Get(int productId)
    {
        return productRepository.Get(productId, includeProperties: "Category") ?? new Product();
    }

    public void Add(Product product)
    {
        productRepository.Add(product);
        productRepository.Save();
    }

    public void Update(Product product)
    {
        productRepository.Update(product);
        productRepository.Save();
    }
}