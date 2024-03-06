using Microsoft.AspNetCore.Mvc;

namespace PureBakes.Areas.Admin.Controllers;

using Microsoft.AspNetCore.Mvc.Rendering;
using PureBakes.Models;
using PureBakes.Service.Services.Interface;

[Area("Admin")]
public class ProductController(
    IProductService productService,
    ICategoryService categoryService,
    IWebHostEnvironment webHostEnvironment) : Controller
{
    public IActionResult Index()
    {
        var allProducts = productService.GetAll();
        return View(allProducts);
    }

    public IActionResult Upsert(int? productId)
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
        productViewModel.Product = product;

        return View(productViewModel);
    }

    [HttpPost]
    public IActionResult Upsert(Product product, IFormFile? file)
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
        return RedirectToAction(nameof(Index));
    }

    [HttpDelete]
    public IActionResult Delete(int productId)
    {
        var deletedSuccessfully = productService.Remove(productId);

        if (!deletedSuccessfully)
        {
            return Json(new { success = false, message = "Error while deleting" });
        }

        return Json(new { success = true, message = "Delete Successful" });
    }
}