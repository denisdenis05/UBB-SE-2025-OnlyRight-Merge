using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WinUIApp.ProxyServices;
using WinUIApp.WebUI.Models;

namespace WinUIApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        private readonly IDrinkService drinkService;

        public HomeController(ILogger<HomeController> logger,IDrinkService drinkService)
        {
            this.drinkService = drinkService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var drinkOfTheDay = drinkService.GetDrinkOfTheDay();
            
            var homeViewModel = new HomeViewModel
            {
                DrinkOfTheDay = drinkOfTheDay
            };
            
            return View(homeViewModel);
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
