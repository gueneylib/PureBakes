namespace PureBakes.Areas.Customer.Controllers;

using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PureBakes.Data.Repository.Interface;
using PureBakes.Models;
using PureBakes.Service.Services.Interface;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    // TODO drop this UnitOfWork. Use services for each repo instead
    private readonly IUnitOfWork _unitOfWork;
    private readonly IShoppingCartService _shoppingCartService;

    public HomeController(
        ILogger<HomeController> logger,
        IUnitOfWork unitOfWork,
        IShoppingCartService shoppingCartService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _shoppingCartService = shoppingCartService;
    }

    public IActionResult Index()
    {
        var allProducts = _unitOfWork.Product.GetAll(includeProperties: "Category");
        return View(allProducts);
    }

    public IActionResult Details(int productId)
    {
        var product = _unitOfWork.Product.Get(productId);
        var item = new ShoppingCartItem
        {
            Product = product,
            Quantity = 1
        };
        return View(item);
    }

    [HttpPost]
    public IActionResult Details(ShoppingCartItem shoppingCartItem)
    {
        if (User.Identity is not ClaimsIdentity claimsIdentity)
        {
            return View(shoppingCartItem);
        }

        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return View(shoppingCartItem);
        }

        // In the service, make sure user has a cart or create one
        var shoppingCartOfUser = _shoppingCartService.GetShoppingCartByUserId(userId);

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
        _unitOfWork.ShoppingCartItem.Update(match);
        _unitOfWork.Save();
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