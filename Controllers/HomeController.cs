using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ThinkfulApp.Models;
using ThinkfulApp.Services;

namespace ThinkfulApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (TempData.ContainsKey("UserId"))
            {
                ViewBag.User = UsersDAO.GetUserById((int)TempData.Peek("UserId"));
                if (ViewBag.User == null)
                {
                    return View("HomeIndex");
                }
                ViewBag.Chart = ChartDataDAO.GetChartFromId((int)TempData.Peek("UserId"));
                if (ViewBag.Chart == null)
                {
                    return View("HomeIndex");
                }
                return View("Index");
            }
            return View("HomeIndex");
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
}