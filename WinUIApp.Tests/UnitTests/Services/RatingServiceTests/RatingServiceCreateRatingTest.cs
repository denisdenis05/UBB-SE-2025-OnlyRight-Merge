using Moq;
using WinUIApp.WebAPI.Models;
using WinUIApp.WebAPI.Repositories;
using WinUIApp.WebAPI.Services;
using WinUIApp.WebAPI.Constants.ErrorMessages;
using Xunit;

namespace WinUIApp.Tests.UnitTests.Services
{
    public class RatingServiceCreateRatingTest
    {
        private readonly RatingService _service;
        private readonly Mock<IRatingRepository> _repository;

        public RatingServiceCreateRatingTest()
        {
            _repository = new Mock<IRatingRepository>();
            _service = new RatingService(_repository.Object);
        }

        [Fact]
        public void CreateRating_WhenValidRating_ReturnsExpectedModel()
        {
            // Arrange
            var rating = new Rating { DrinkId = 1, UserId = 1, RatingValue = 4 };

            var expectedDataModel = rating.ToDataModel();
            var returnedDataModel = new WinUiApp.Data.Data.Rating
            {
                RatingId = 10,
                DrinkId = 1,
                UserId = 1,
                RatingValue = 4,
                RatingDate = DateTime.UtcNow,
                IsActive = true
            };

            _repository.Setup(repo => repo.AddRating(It.IsAny<WinUiApp.Data.Data.Rating>()))
                .Returns(returnedDataModel);

            // Act
            var result = _service.CreateRating(rating);

            // Assert
            Assert.Equal(returnedDataModel.DrinkId, result.DrinkId);
            Assert.Equal(returnedDataModel.UserId, result.UserId);
        }

        [Fact]
        public void CreateRating_WhenInvalidRating_ThrowsArgumentException()
        {
            // Arrange
            var rating = new Rating { DrinkId = 1, UserId = 1, RatingValue = 7 }; // invalid

            // Act
            var ex = Assert.Throws<ArgumentException>(() => _service.CreateRating(rating));

            // Assert
            Assert.Equal(ServiceErrorMessages.InvalidRatingValue, ex.Message);
        }

        [Fact]
        public void CreateRating_WhenValidRating_CallsRepositoryOnce()
        {
            // Arrange
            var rating = new Rating { DrinkId = 1, UserId = 1, RatingValue = 4 };

            _repository.Setup(repo => repo.AddRating(It.IsAny<WinUiApp.Data.Data.Rating>()))
                .Returns(rating.ToDataModel());

            // Act
            _service.CreateRating(rating);

            // Assert
            _repository.Verify(repo => repo.AddRating(It.IsAny<WinUiApp.Data.Data.Rating>()), Times.Once);
        }
    }
}
