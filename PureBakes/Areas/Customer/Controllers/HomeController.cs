namespace PureBakes.Areas.Customer.Controllers;

using System.Diagnostics;
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
    : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    public IActionResult Index()
    {
        var allProducts = productService.GetAll();
        return View(allProducts);
    }

    public IActionResult Details(int productId)
    {
        var product = productService.Get(productId);
        var item = new ShoppingCartItem
        {
            Product = product,
            Quantity = 1
        };
        return View(item);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Details(ShoppingCartItem shoppingCartItem)
    {
        var userId = identityService.GetUserId();
        if (string.IsNullOrWhiteSpace(userId))
        {
            return View(shoppingCartItem);
        }

        var shoppingCartOfUser = shoppingCartService.GetShoppingCartByUserId(userId);

        // TODO create factory
        var match = shoppingCartOfUser.ShoppingCartItem.FirstOrDefault(x => x.ProductId == shoppingCartItem.ProductId);
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
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}