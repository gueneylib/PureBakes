using Microsoft.AspNetCore.Mvc;

namespace PureBakes.Areas.Admin.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using PureBakes.Models;
using PureBakes.Service.Constants;
using PureBakes.Service.Services.Interface;

[Area("Admin")]
[Authorize(Roles = RoleConstants.Admin)]
public class ProductController(
    ILogger<ProductController> logger,
    IProductService productService,
    ICategoryService categoryService,
    IWebHostEnvironment webHostEnvironment)
    : PureBakesBaseController(logger)
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
            logger.LogError(ex, ex.Message);
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
            logger.LogError(ex, ex.Message);
            return RedirectToAction(nameof(Error));
        }
    }

    [HttpPost]
    public IActionResult Upsert(Product product, IFormFile? file)
    {
        try
        {
            if (product.Title.ToLower().Contains("cake"))
            {
                ModelState.AddModelError("Product.Title", "Cake is not supported in PureBakes!");
            }
            if (!ModelState.IsValid)
            {
                var categories = categoryService.GetAll().Select(x => new SelectListItem(x.Name, x.Id.ToString()));
                var productViewModel = new ProductViewModel
                {
                    Product = product,
                    CategoryList = categories
                };
                return View(productViewModel);
            }
            string wwwRootPath = webHostEnvironment.WebRootPath;
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPath, $@"images{Path.DirectorySeparatorChar}product");

                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    //delete the old image
                    var oldImagePath =
                        Path.Combine(wwwRootPath, product.ImageUrl.TrimStart(Path.DirectorySeparatorChar));

                    // TODO create service for file operations
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName),FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                product.ImageUrl = $@"{Path.DirectorySeparatorChar}images{Path.DirectorySeparatorChar}product{Path.DirectorySeparatorChar}" + fileName;
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
            logger.LogError(ex, ex.Message);
            TempData["error"] = $"Something went wrong: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
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
            logger.LogError(ex, ex.Message);
            return Json(new { success = false, message = "Error while deleting" });
        }
    }
}