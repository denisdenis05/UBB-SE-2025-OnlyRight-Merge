using Microsoft.AspNetCore.Mvc;
using WinUIApp.ProxyServices;
using WinUIApp.ProxyServices.Models;

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

            var viewModel = new DrinkDetailViewModel
            {
                Drink = drink,
                CategoriesDisplay = string.Join(", ", drink.CategoryList.Select(c => c.CategoryName)),
                AverageRatingScore = ratingService.GetAverageRating(id),
                Reviews = reviews,
            };

            return View(viewModel);
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