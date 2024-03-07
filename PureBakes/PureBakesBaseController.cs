namespace PureBakes;

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PureBakes.Models;

public class PureBakesBaseController(ILogger<PureBakesBaseController> logger) : Controller
{
    private ILogger<PureBakesBaseController>  _logger { get; set; } = logger;

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}