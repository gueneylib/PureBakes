namespace PureBakes.Service.Services;

using PureBakes.Data.Repository.Interface;
using PureBakes.Models;
using PureBakes.Service.Services.Interface;

public class ProductService(
    IProductRepository productRepository,
    IFileService fileService) : IProductService
{
    public IEnumerable<Product> GetAll()
    {
        return productRepository.GetAll(includeProperties: "Category");
    }

    public Product? Get(int productId)
    {
        return productRepository.Get(productId, includeProperties: "Category");
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

    public bool Remove(int productId)
    {
        var product = Get(productId);
        if (product is null)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(product.ImageUrl))
        {
            fileService.RemoveOldImageIfExists(product.ImageUrl);
        }

        productRepository.Remove(product);
        productRepository.Save();

        return true;
    }

    public string GetImageUrlOfProduct(int productId)
    {
        var product = Get(productId);
        if (product is not null)
        {
            return product.ImageUrl ?? string.Empty;
        }

        return string.Empty;
    }
}