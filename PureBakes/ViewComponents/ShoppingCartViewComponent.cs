namespace PureBakes.ViewComponents;

using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PureBakes.Data.Repository.Interface;
using PureBakes.Service.Services.Interface;

public class ShoppingCartViewComponent : ViewComponent
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IShoppingCartService _shoppingCartService;

    public ShoppingCartViewComponent(IUnitOfWork unitOfWork, IShoppingCartService shoppingCartService)
    {
        _unitOfWork = unitOfWork;
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

            var shoppingCartOfUser = _shoppingCartService.GetShoppingCartByUserId(userId);
            var currentCartProductsCount = _unitOfWork.ShoppingCartItem
                .GetAll(u => u.ShoppingCartId == shoppingCartOfUser.Id)
                .Sum(product => product.Quantity);
            HttpContext.Session.SetInt32("SessionCart", currentCartProductsCount);

            return View(currentCartProductsCount);

        }

        HttpContext.Session.Clear();
        return View(0);

    }

}