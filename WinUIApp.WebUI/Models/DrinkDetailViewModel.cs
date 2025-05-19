using System.Collections.Generic;
using WinUIApp.ProxyServices.Models;

namespace WinUIApp.WebUI.Models
{
    public class DrinkDetailViewModel
    {
        public Drink Drink { get; set; }
        public string CategoriesDisplay { get; set; }
        public double AverageRatingScore { get; set; }
        public List<Review> Reviews { get; set; }
        public bool IsInFavorites { get; set; }
    }
}