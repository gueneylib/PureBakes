namespace PureBakes.Areas.Customer.Controllers;

using Microsoft.AspNetCore.Mvc;
using PureBakes.Data.Repository.Interface;

[Area("Customer")]
public class CartController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CartController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        var allProducts = _unitOfWork.Product.GetAll(includeProperties: "Category");
        return View(allProducts);
    }
}