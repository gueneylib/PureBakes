namespace PureBakes.Service.Services;

using Microsoft.AspNetCore.Hosting;
using PureBakes.Data.Repository.Interface;
using PureBakes.Models;
using PureBakes.Service.Services.Interface;

// TODO webhostenvironment has frameworkreference to asp.net. does this really belong to the service layer?
public class ProductService(
    IProductRepository productRepository,
    IWebHostEnvironment webHostEnvironment) : IProductService
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

        if (!string.IsNullOrEmpty(product.ImageUrl))
        {
            var oldImagePath =
                Path.Combine(webHostEnvironment.WebRootPath,
                    product.ImageUrl.TrimStart(Path.DirectorySeparatorChar));

            // TODO create service for file operations
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
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