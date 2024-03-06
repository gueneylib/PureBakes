namespace PureBakes.Areas.Customer.Controllers;

using Microsoft.AspNetCore.Mvc;
using PureBakes.Models;
using PureBakes.Service.Constants;
using PureBakes.Service.Services.Interface;

[Area("Customer")]
public class CartController(
    IShoppingCartService shoppingCartService) : Controller
{

    public IActionResult Index()
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

    public IActionResult Plus(int cartItemId)
    {
        var incrementSuccessful = shoppingCartService.IncrementProductQuantity(cartItemId);
        UpdateSessionCartCountIfOperationSuccessful(incrementSuccessful);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Minus(int cartItemId)
    {
        var decrementSuccessful = shoppingCartService.DecrementProductQuantity(cartItemId);
        UpdateSessionCartCountIfOperationSuccessful(decrementSuccessful);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Remove(int cartItemId)
    {
        var removeSuccessful = shoppingCartService.RemoveProductFromCart(cartItemId);
        UpdateSessionCartCountIfOperationSuccessful(removeSuccessful);
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