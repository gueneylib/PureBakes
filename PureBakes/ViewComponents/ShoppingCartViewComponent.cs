namespace PureBakes.ViewComponents;

using Microsoft.AspNetCore.Mvc;
using PureBakes.Service.Constants;
using PureBakes.Service.Services.Interface;

public class ShoppingCartViewComponent(
    IShoppingCartService shoppingCartService,
    IIdentityService identityService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = identityService.GetUserId();
        if (string.IsNullOrWhiteSpace(userId))
        {
            HttpContext.Session.Clear();
            return View(0);
        }

        if (HttpContext.Session.GetInt32(SessionConstants.SessionCartCount) != null)
        {
            return View(HttpContext.Session.GetInt32(SessionConstants.SessionCartCount) ?? 0);
        }

        var shoppingCartProductsQuantity = shoppingCartService.GetShoppingCartProductsQuantity();
        HttpContext.Session.SetInt32(SessionConstants.SessionCartCount, shoppingCartProductsQuantity);

        return View(shoppingCartProductsQuantity);
    }
}