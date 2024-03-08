namespace PureBakes.Areas.Customer.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PureBakes.Models;
using PureBakes.Service.Constants;
using PureBakes.Service.Services.Interface;

[Area("Customer")]
[Authorize]
public class CartController(
    ILogService<CartController> logService,
    IShoppingCartService shoppingCartService)
    : PureBakesBaseController(logService)
{

    public IActionResult Index()
    {
        try
        {
            var allProducts = shoppingCartService.GetAllProductsInCart();
            foreach (var item in allProducts)
            {
                item.TotalPrice = item.Quantity * item.Product?.Price ?? 0;
            }

            var cartViewModel = new CartViewModel
            {
                ShoppingCartItems = allProducts,
                TotalCartPrice = allProducts.Sum(x => x.TotalPrice),
            };
            return View(cartViewModel);
        }
        catch (Exception ex)
        {
            logService.LogError(ex, ex.Message);
            return RedirectToAction(nameof(Error));
        }
    }

    public IActionResult Plus(int cartItemId)
    {
        try
        {
            var incrementSuccessful = shoppingCartService.IncrementProductQuantity(cartItemId);
            UpdateSessionCartCountIfOperationSuccessful(incrementSuccessful);
        }
        catch (Exception ex)
        {
            logService.LogError(ex, ex.Message);
            TempData["error"] = $"Something went wrong: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Minus(int cartItemId)
    {
        try
        {
            var decrementSuccessful = shoppingCartService.DecrementProductQuantity(cartItemId);
            UpdateSessionCartCountIfOperationSuccessful(decrementSuccessful);
        }
        catch (Exception ex)
        {
            logService.LogError(ex, ex.Message);
            TempData["error"] = $"Something went wrong: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Remove(int cartItemId)
    {
        try
        {
            var removeSuccessful = shoppingCartService.RemoveProductFromCart(cartItemId);
            UpdateSessionCartCountIfOperationSuccessful(removeSuccessful);
        }
        catch (Exception ex)
        {
            logService.LogError(ex, ex.Message);
            TempData["error"] = $"Something went wrong: {ex.Message}";
        }
        return RedirectToAction(nameof(Index));
    }

    private void UpdateSessionCartCountIfOperationSuccessful(bool operationSuccessful)
    {
        if (operationSuccessful)
        {
            var currentCartProductsCount = shoppingCartService.GetShoppingCartProductsQuantity();
            HttpContext.Session.SetInt32(SessionConstants.SessionCartCount, currentCartProductsCount);
        }
    }
}