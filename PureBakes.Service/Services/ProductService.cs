namespace PureBakes.Service.Services;

using PureBakes.Data.Repository.Interface;
using PureBakes.Models;
using PureBakes.Service.Constants;
using PureBakes.Service.Services.Interface;

public class ProductService(
    IProductRepository productRepository,
    IFileService fileService,
    IIdentityService identityService) : IProductService
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

        if (UserHasNoPermissionForProduct(product.ImageUrl ?? string.Empty))
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

    public bool UserHasNoPermissionForProduct(string imageUrl)
    {
        var role = identityService.GetUserRole();
        if (imageUrl == "/images/product/ryeSourdough.jpg" ||
            imageUrl == "/images/product/ciabatta.jpg" ||
            imageUrl == "/images/product/neapolitan.jpeg" ||
            imageUrl == "/images/product/romanPizza.jpg" ||
            imageUrl == "/images/product/simit.jpg" ||
            imageUrl == "/images/product/brezel.jpg" &&
            role != RoleConstants.SuperAdmin)
        {
            return true;
        }

        return false;
    }
}