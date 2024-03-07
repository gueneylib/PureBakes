namespace PureBakes.Areas.Customer.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PureBakes.Models;
using PureBakes.Service.Constants;
using PureBakes.Service.Services.Interface;

[Area("Customer")]
public class HomeController(
    ILogger<HomeController> logger,
    IShoppingCartService shoppingCartService,
    IProductService productService,
    IIdentityService identityService)
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
            logger.LogError(ex, ex.Message);
            return RedirectToAction(nameof(Error));
        }
    }

    [HttpPost]
    [Authorize]
    public IActionResult Details(ShoppingCartItem shoppingCartItem)
    {
        try
        {
            var userId = identityService.GetUserId();
            if (string.IsNullOrWhiteSpace(userId))
            {
                return View(shoppingCartItem);
            }

            var shoppingCartOfUser = shoppingCartService.GetShoppingCartByUserId(userId);

            // TODO create factory
            var match = shoppingCartOfUser.ShoppingCartItem.FirstOrDefault(x =>
                x.ProductId == shoppingCartItem.ProductId);
            if (match is not null)
            {
                match.Quantity += shoppingCartItem.Quantity;
            }
            else
            {
                match = new ShoppingCartItem
                {
                    ProductId = shoppingCartItem.ProductId,
                    Quantity = shoppingCartItem.Quantity,
                    ShoppingCartId = shoppingCartOfUser.Id
                };
                // shoppingCartOfUser.ShoppingCartItem.Add(match);
            }

            shoppingCartService.UpdateCartItem(match);

            var currentCartProductsCount = shoppingCartService.GetShoppingCartProductsQuantity();
            HttpContext.Session.SetInt32(SessionConstants.SessionCartCount, currentCartProductsCount);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
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
            logger.LogError(ex, ex.Message);
            return RedirectToAction(nameof(Error));
        }
    }
}