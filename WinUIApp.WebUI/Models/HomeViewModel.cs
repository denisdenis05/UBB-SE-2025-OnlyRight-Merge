using Drink = WinUIApp.ProxyServices.Models.Drink;

namespace WinUIApp.WebUI.Models
{
    public class HomeViewModel
    {
        public required Drink DrinkOfTheDay { get; set; }
    }
}