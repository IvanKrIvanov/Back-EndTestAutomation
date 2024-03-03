using Microsoft.AspNetCore.Mvc;
using MVCIntroDemo.Models;
using System.Diagnostics;

namespace MVCIntroDemo.Controllers
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
            ViewBag.Message = "Hello Ivan!";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewBag.AnotherMessage = "This is an ASP.NET core MVC App";
            return View();
        }
		public IActionResult Numbers()
		{
			return View();
		}
		public IActionResult NumbersToN(int count = 3)
		{
            ViewBag.Count = count;
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
