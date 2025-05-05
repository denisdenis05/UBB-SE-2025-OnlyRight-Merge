using Moq;
using WinUIApp.WebAPI.Models;
using WinUIApp.WebAPI.Repositories;
using WinUIApp.WebAPI.Services;
using WinUiApp.Data.Data;
using Xunit;

namespace WinUIApp.Tests.UnitTests.Services
{
    public class RatingServiceGetRatingByIdTest
    {
        private readonly RatingService _service;
        private readonly Mock<IRatingRepository> _repository;

        public RatingServiceGetRatingByIdTest()
        {
            _repository = new Mock<IRatingRepository>();
            _service = new RatingService(_repository.Object);
        }

        [Fact]
        public void GetRatingById_WhenRatingExists_ReturnsRating()
        {
            // Arrange
            var ratingId = 1;
            var expectedDataModel = new WinUiApp.Data.Data.Rating
            {
                RatingId = ratingId
            };

            _repository.Setup(repo => repo.GetRatingById(ratingId))
                       .Returns(expectedDataModel);

            // Act
            var result = _service.GetRatingById(ratingId);

            // Assert
            Assert.Equal(ratingId, result.RatingId);
        }

        [Fact]
        public void GetRatingById_WhenRatingDoesNotExist_ReturnsNull()
        {
            // Arrange
            var ratingId = 999;

            _repository.Setup(repo => repo.GetRatingById(ratingId))
                       .Returns((WinUiApp.Data.Data.Rating)null);

            // Act
            var result = _service.GetRatingById(ratingId);

            // Assert
            Assert.Null(result);
        }
    }
}
