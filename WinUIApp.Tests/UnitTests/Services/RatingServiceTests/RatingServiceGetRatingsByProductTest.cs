using Moq;
using WinUiApp.Data.Data;
using WinUIApp.WebAPI.Repositories;
using WinUIApp.WebAPI.Services;
using System.Collections.Generic;
using Xunit;

namespace WinUIApp.Tests.UnitTests.Services
{
    public class RatingServiceGetRatingsByDrinkTest
    {
        private readonly RatingService _service;
        private readonly Mock<IRatingRepository> _repository;

        public RatingServiceGetRatingsByDrinkTest()
        {
            _repository = new Mock<IRatingRepository>();
            _service = new RatingService(_repository.Object);
        }

        private const int DrinkId = 1;

        [Fact]
        public void GetRatingsByDrink_WhenRatingsExist_ReturnsCorrectCount()
        {
            // Arrange
            var ratings = new List<Rating> { new Rating(), new Rating() };
            _repository.Setup(repo => repo.GetRatingsByDrinkId(DrinkId)).Returns(ratings);

            // Act
            var result = _service.GetRatingsByDrink(DrinkId);

            // Assert
            Assert.Equal(ratings.Count, result.Count()); // Only checking the count of ratings returned
        }

        [Fact]
        public void GetRatingsByDrink_WhenNoRatingsExist_ReturnsEmptyCollection()
        {
            // Arrange
            _repository.Setup(repo => repo.GetRatingsByDrinkId(DrinkId)).Returns(new List<Rating>());

            // Act
            var result = _service.GetRatingsByDrink(DrinkId);

            // Assert
            Assert.Empty(result); // Only checking if the result is empty
        }
    }
}
