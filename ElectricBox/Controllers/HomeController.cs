using ElectricBox.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ElectricBox.Controllers
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
            var db = new CreateAppDB();
            db.CreateDB();

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contacts()
        {
            return View();
        }

        //public IActionResult Kontur()
        //{
        //    return View();
        //}

        public IActionResult Cable()
        {
            return View();
        }

        public IActionResult Apparat()
        {
            return View();
        }

        public IActionResult Transformator()
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