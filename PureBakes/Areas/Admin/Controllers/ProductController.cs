using Microsoft.AspNetCore.Mvc;

namespace PureBakes.Areas.Admin.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using PureBakes.Models;
using PureBakes.Service.Constants;
using PureBakes.Service.Services.Interface;

[Area("Admin")]
[Authorize(Roles = $"{RoleConstants.Admin},{RoleConstants.SuperAdmin}")]
public class ProductController(
    ILogService<ProductController> logService,
    IProductService productService,
    ICategoryService categoryService,
    IFileService fileService)
    : PureBakesBaseController(logService)
{
    public IActionResult Index()
    {
        try
        {
            var allProducts = productService.GetAll();
            return View(allProducts);
        }
        catch (Exception ex)
        {
            logService.LogError(ex, ex.Message);
            return RedirectToAction(nameof(Error));
        }
    }

    public IActionResult Upsert(int? productId)
    {
        try
        {
            var categories = categoryService.GetAll().Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            var productViewModel = new ProductViewModel()
            {
                CategoryList = categories
            };
            if (productId is null)
            {
                return View(productViewModel);
            }

            var product = productService.Get(productId.GetValueOrDefault());
            productViewModel.Product = product ?? new Product();

            return View(productViewModel);
        }
        catch (Exception ex)
        {
            logService.LogError(ex, ex.Message);
            return RedirectToAction(nameof(Error));
        }
    }

    [HttpPost]
    public IActionResult Upsert(Product product, IFormFile? file)
    {
        try
        {
            if (productService.UserHasNoPermissionForProduct(product.ImageUrl ?? string.Empty))
            {
                return LocalRedirect("/Identity/Account/AccessDenied");
            }

            // This is just to demonstrate custom validation.
            if (product.Title.ToLower().Contains("cake"))
            {
                ModelState.AddModelError("Product.Title", "Cake is not supported in PureBakes!");
            }

            if (!ModelState.IsValid)
            {
                var productViewModel = CreateProductViewModel(product);
                return View(productViewModel);
            }

            if (file != null)
            {
                fileService.RemoveOldImageIfExists(product.ImageUrl);
                product.ImageUrl = fileService.AddImageToProduct(file.OpenReadStream(), file.FileName).GetAwaiter().GetResult();
            }
            if (product.Id == 0)
            {
                TempData["success"] = "Product added successfully";
                productService.Add(product);
            }
            else
            {
                TempData["success"] = "Product updated successfully";
                productService.Update(product);
            }
        }
        catch (Exception ex)
        {
            logService.LogError(ex, ex.Message);
            TempData["error"] = $"Something went wrong: {ex.Message}";

            var productViewModel = CreateProductViewModel(product);
            return View(productViewModel);
        }

        return RedirectToAction(nameof(Index));
    }

    private ProductViewModel CreateProductViewModel(Product product)
    {
        var categories = categoryService.GetAll().Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        var productViewModel = new ProductViewModel
        {
            Product = product,
            CategoryList = categories
        };
        return productViewModel;
    }

    [HttpDelete]
    public IActionResult Delete(int productId)
    {
        try
        {
            var deletedSuccessfully = productService.Remove(productId);

            if (!deletedSuccessfully)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            return Json(new { success = true, message = "Delete Successful" });
        }
        catch (Exception ex)
        {
            logService.LogError(ex, ex.Message);
            return Json(new { success = false, message = "Error while deleting" });
        }
    }
}