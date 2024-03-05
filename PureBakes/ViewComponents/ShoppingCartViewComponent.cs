namespace PureBakes.ViewComponents;

using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PureBakes.Data.Repository.Interface;
using PureBakes.Service.Services.Interface;

public class ShoppingCartViewComponent : ViewComponent
{
    private readonly IShoppingCartService _shoppingCartService;

    public ShoppingCartViewComponent(IShoppingCartService shoppingCartService)
    {
        _shoppingCartService = shoppingCartService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        if (claim != null) {
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return View(0);
            }

            if (HttpContext.Session.GetInt32("SessionCart") != null)
            {
                return View(HttpContext.Session.GetInt32("SessionCart") ?? 0);
            }

            var shoppingCartProductsQuantity = _shoppingCartService.GetShoppingCartProductsQuantity();
            HttpContext.Session.SetInt32("SessionCart", shoppingCartProductsQuantity);

            return View(shoppingCartProductsQuantity);

        }

        HttpContext.Session.Clear();
        return View(0);

    }

}