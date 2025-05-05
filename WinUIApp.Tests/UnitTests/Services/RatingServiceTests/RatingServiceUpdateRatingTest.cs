using Moq;
using WinUIApp.WebAPI.Models;
using WinUIApp.WebAPI.Repositories;
using WinUIApp.WebAPI.Services;
using WinUIApp.WebAPI.Constants.ErrorMessages;
using Xunit;

namespace WinUIApp.Tests.UnitTests.Services
{
    public class RatingServiceUpdateRatingTest
    {
        private readonly RatingService _service;
        private readonly Mock<IRatingRepository> _repository;

        public RatingServiceUpdateRatingTest()
        {
            _repository = new Mock<IRatingRepository>();
            _service = new RatingService(_repository.Object);
        }

        [Fact]
        public void UpdateRating_WhenValidRating_ReturnsUpdatedModel()
        {
            // Arrange
            var inputModel = new Rating { DrinkId = 1, UserId = 1, RatingValue = 4 };
            var returnedDataModel = new WinUiApp.Data.Data.Rating
            {
                RatingId = 10,
                DrinkId = 1,
                UserId = 1,
                RatingValue = 4,
                RatingDate = DateTime.UtcNow,
                IsActive = true
            };

            _repository.Setup(repo => repo.UpdateRating(It.IsAny<WinUiApp.Data.Data.Rating>()))
                       .Returns(returnedDataModel);

            // Act
            var result = _service.UpdateRating(inputModel);

            // Assert
            Assert.Equal(returnedDataModel.RatingId, result.RatingId);
            Assert.Equal(returnedDataModel.DrinkId, result.DrinkId);
            Assert.Equal(returnedDataModel.UserId, result.UserId);
        }

        [Fact]
        public void UpdateRating_WhenValidRating_CallsRepositoryOnce()
        {
            // Arrange
            var rating = new Rating { DrinkId = 1, UserId = 1, RatingValue = 4 };

            _repository.Setup(repo => repo.UpdateRating(It.IsAny<WinUiApp.Data.Data.Rating>()))
                       .Returns(rating.ToDataModel());

            // Act
            _service.UpdateRating(rating);

            // Assert
            _repository.Verify(repo => repo.UpdateRating(It.IsAny<WinUiApp.Data.Data.Rating>()), Times.Once);
        }

        [Fact]
        public void UpdateRating_WhenInvalidRating_ThrowsArgumentException()
        {
            // Arrange
            var invalidRating = new Rating { DrinkId = 1, UserId = 1, RatingValue = 8 };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _service.UpdateRating(invalidRating));
            Assert.Equal(ServiceErrorMessages.InvalidRatingValue, ex.Message);
        }
    }
}
