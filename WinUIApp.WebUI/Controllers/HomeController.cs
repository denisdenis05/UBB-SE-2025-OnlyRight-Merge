using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WinUIApp.ProxyServices;
using WinUIApp.ProxyServices.Models;
using WinUIApp.WebUI.Models;


namespace WinUIApp.WebUI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    
    private readonly IDrinkService drinkService;

    public HomeController(ILogger<HomeController> logger,IDrinkService drinkService)
    {
        _logger = logger;
        this.drinkService = drinkService;
    }
    
    public IActionResult Index()
    {
        var drinkOfTheDay = drinkService.GetDrinkOfTheDay();
        var drinkCategories = drinkService.GetDrinkCategories();
        var drinkBrands = drinkService.GetDrinkBrandNames();
        var drinks = new List<Drink>();

        var homeViewModel = new HomeViewModel
        {
            DrinkOfTheDay = drinkOfTheDay,
            drinkCategories = drinkCategories,
            drinkBrands = drinkBrands,
            drinks = drinks
        };
        
        return View(homeViewModel);
    }
    
    // Filtering products
    [HttpPost]
    public IActionResult Index(string? searchKeyword, float? minValue, float? maxValue, int? minStars,string[] SelectedCategories,string[] SelectedBrandNames)
    {
        var drinkOfTheDay = drinkService.GetDrinkOfTheDay();
        var drinkCategories = drinkService.GetDrinkCategories();
        var drinkBrands = drinkService.GetDrinkBrandNames();
        
        List<string> drinkCategoriesList = SelectedCategories.Select(s => (string?)s ?? "").ToList();
        List<string> drinkBrandsList = SelectedBrandNames.Select(s => (string?)s ?? "").ToList();
        
        drinkCategoriesList.ForEach(drink=>_logger.LogInformation(drink.ToString()));
        Dictionary<string,bool> drinkOrderingCriteria = new Dictionary<string,bool>();

        var drinks = drinkService.GetDrinks(searchKeyword,drinkBrandsList ,drinkCategoriesList,minValue, maxValue,drinkOrderingCriteria);
        
        var homeViewModel = new HomeViewModel
        {
            DrinkOfTheDay = drinkOfTheDay,
            drinkCategories = drinkCategories,
            drinkBrands = drinkBrands,
            drinks = drinks
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
