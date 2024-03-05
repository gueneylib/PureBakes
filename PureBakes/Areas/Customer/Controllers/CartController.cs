namespace PureBakes.Areas.Customer.Controllers;

using Microsoft.AspNetCore.Mvc;
using PureBakes.Data.Repository.Interface;
using PureBakes.Service.Services.Interface;

[Area("Customer")]
public class CartController(
    IShoppingCartService shoppingCartService,
    IProductService productService) : Controller
{

    public IActionResult Index()
    {
        var allProducts = productService.GetAll();
        return View(allProducts);
    }
}