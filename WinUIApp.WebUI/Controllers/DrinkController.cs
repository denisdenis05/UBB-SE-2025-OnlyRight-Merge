using Microsoft.AspNetCore.Mvc;
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
            if (drink == null)
            {
                return NotFound();
            }
            
            var ratings = ratingService.GetRatingsByProduct(id);
            var reviews = new List<Review>();
            var reviewsByRating = new Dictionary<int, List<Review>>();
            
            foreach (var rating in ratings)
            {
                var ratingReviews = reviewService.GetReviewsByRating(rating.RatingId).ToList();
                reviews.AddRange(ratingReviews);
                
                // Group reviews by rating ID
                reviewsByRating[rating.RatingId] = ratingReviews;
            }

            var viewModel = new DrinkDetailViewModel
            {
                Drink = drink,
                CategoriesDisplay = drink.CategoryList != null 
                    ? string.Join(", ", drink.CategoryList.Select(c => c.CategoryName)) 
                    : string.Empty,
                AverageRatingScore = ratingService.GetAverageRating(id),
                Ratings = ratings.ToList(),
                Reviews = reviews,
                ReviewsByRating = reviewsByRating,
                NewReview = new RatingReviewViewModel { DrinkId = id }
            };
            
            return View(viewModel);
        }        [HttpGet]
        public IActionResult AddReviewToRating(int ratingId, int drinkId)
        {
            var rating = ratingService.GetRatingById(ratingId);
            if (rating == null)
            {
                TempData["ErrorMessage"] = "Rating not found.";
                return RedirectToAction("DrinkDetail", new { id = drinkId });
            }
            
            var viewModel = new RatingReviewViewModel
            {
                RatingId = ratingId,
                DrinkId = drinkId,
                Score = (int)rating.RatingValue
            };
            
            TempData["RatingValue"] = rating.RatingValue;
            
            return View(viewModel);
        }
        
        [HttpPost]
        public IActionResult AddRatingAndReview(RatingReviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // If there are validation errors, redirect back to the drink detail page
                TempData["ErrorMessage"] = "Please fill in all required fields.";
                if (model.RatingId.HasValue)
                {
                    return RedirectToAction("AddReviewToRating", new { ratingId = model.RatingId.Value, drinkId = model.DrinkId });
                }
                return RedirectToAction("DrinkDetail", new { id = model.DrinkId });
            }
            
            try
            {
                Rating savedRating;
                
                // If RatingId is provided, add a review to existing rating
                if (model.RatingId.HasValue)
                {
                    // Use existing rating
                    var rating = ratingService.GetRatingById(model.RatingId.Value);
                    if (rating == null)
                    {
                        TempData["ErrorMessage"] = "The selected rating could not be found.";
                        return RedirectToAction("DrinkDetail", new { id = model.DrinkId });
                    }
                    
                    savedRating = rating;
                }
                else
                {
                    // Create new rating
                    var rating = new Rating
                    {
                        DrinkId = model.DrinkId,
                        UserId = model.UserId,
                        RatingValue = model.Score,
                        RatingDate = DateTime.Now,
                        IsActive = true
                    };
                    
                    savedRating = ratingService.CreateRating(rating);
                }
                  // Create and save the review only if content is provided
                if (!string.IsNullOrWhiteSpace(model.ReviewContent))
                {
                    var review = new Review
                    {
                        RatingId = savedRating.RatingId,
                        UserId = model.UserId,
                        Content = model.ReviewContent
                    };
                    
                    reviewService.AddReview(review);
                    TempData["SuccessMessage"] = "Your rating and review have been submitted successfully.";
                }
                else
                {
                    TempData["SuccessMessage"] = "Your rating has been submitted successfully.";
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "There was an error submitting your review. Please try again.";
            }
            
            // Redirect back to the drink detail page
            return RedirectToAction("DrinkDetail", new { id = model.DrinkId });
        }
    }
}