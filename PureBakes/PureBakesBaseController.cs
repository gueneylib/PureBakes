namespace PureBakes;

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PureBakes.Models;
using PureBakes.Service.Services.Interface;

public class PureBakesBaseController(ILogService<PureBakesBaseController> logService) : Controller
{
    private ILogService<PureBakesBaseController>  LogService { get; set; } = logService;

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}