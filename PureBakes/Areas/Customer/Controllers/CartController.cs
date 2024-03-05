namespace PureBakes.Areas.Customer.Controllers;

using Microsoft.AspNetCore.Mvc;
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
        return View(allProducts);
    }
}