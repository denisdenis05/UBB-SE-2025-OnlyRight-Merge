using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WinUIApp.ProxyServices;
using WinUIApp.ProxyServices.Models;
using WinUIApp.WebUI.Models;

namespace WinUIApp.WebUI.Controllers
{
    public class DrinkController : Controller
    {
        private readonly IDrinkService drinkService;
        private readonly IReviewService reviewService;
        private readonly IRatingService ratingService;

        public DrinkController(IDrinkService drinkService, IReviewService reviewService, IRatingService ratingService)
        {
            this.drinkService = drinkService;
            this.reviewService = reviewService;
            this.ratingService = ratingService;
        }

        public IActionResult DrinkDetail(int id)
        {
            var drink = drinkService.GetDrinkById(id);
            var ratings = ratingService.GetRatingsByProduct(id);
            var reviews = new List<Review>();
            foreach (var rating in ratings)
            {
                var ratingReviews = reviewService.GetReviewsByRating(rating.RatingId); 
                reviews.AddRange(ratingReviews);
            }

            const int CurrentUserId = 1; // Using a default user ID for now
            bool isInFavorites = drinkService.IsDrinkInUserPersonalList(CurrentUserId, id);

            var viewModel = new DrinkDetailViewModel
            {
                Drink = drink,
                CategoriesDisplay = string.Join(", ", drink.CategoryList.Select(c => c.CategoryName)),
                AverageRatingScore = ratingService.GetAverageRating(id),
                Reviews = reviews,
                IsInFavorites = isInFavorites
            };

            return View(viewModel);
        }
        
        [HttpPost]
        public IActionResult ToggleFavorites(int id)
        {
            try
            {
                const int CurrentUserId = 1; // Using a default user ID for now
                bool isInFavorites = drinkService.IsDrinkInUserPersonalList(CurrentUserId, id);
                
                if (isInFavorites)
                {
                    drinkService.DeleteFromUserPersonalDrinkList(CurrentUserId, id);
                    TempData["SuccessMessage"] = "Drink removed from favorites.";
                }
                else
                {
                    drinkService.AddToUserPersonalDrinkList(CurrentUserId, id);
                    TempData["SuccessMessage"] = "Drink added to favorites!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error updating favorites.";
            }
            
            return RedirectToAction("DrinkDetail", new { id });
        }
        
        [HttpPost]
        public IActionResult RemoveDrink(int id)
        {
            try
            {
                drinkService.DeleteDrink(id);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("DrinkDetail", new { id });
            }
        }
    }
} 