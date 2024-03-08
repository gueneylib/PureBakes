namespace PureBakes.Areas.Customer.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PureBakes.Models;
using PureBakes.Service.Constants;
using PureBakes.Service.Services.Interface;

[Area("Customer")]
public class HomeController(
    ILogService<HomeController> logService,
    IShoppingCartService shoppingCartService,
    IProductService productService)
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

    public IActionResult Details(int productId)
    {
        try
        {
            var product = productService.Get(productId);
            var item = new ShoppingCartItem
            {
                Product = product,
                Quantity = 1
            };
            return View(item);
        }
        catch (Exception ex)
        {
            logService.LogError(ex, ex.Message);
            return RedirectToAction(nameof(Error));
        }
    }

    [HttpPost]
    [Authorize]
    public IActionResult Details(ShoppingCartItem shoppingCartItem)
    {
        try
        {
            shoppingCartService.UpdateCartItem(shoppingCartItem);

            var currentCartProductsCount = shoppingCartService.GetShoppingCartProductsQuantity();
            HttpContext.Session.SetInt32(SessionConstants.SessionCartCount, currentCartProductsCount);
        }
        catch (Exception ex)
        {
            logService.LogError(ex, ex.Message);
            TempData["error"] = $"Something went wrong: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        try
        {
            return View();
        }
        catch (Exception ex)
        {
            logService.LogError(ex, ex.Message);
            return RedirectToAction(nameof(Error));
        }
    }
}