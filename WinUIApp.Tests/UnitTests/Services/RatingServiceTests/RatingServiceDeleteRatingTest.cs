using Moq;
using WinUIApp.WebAPI.Repositories;
using WinUIApp.WebAPI.Services;
using Xunit;

namespace WinUIApp.Tests.UnitTests.Services
{
    public class RatingServiceDeleteRatingTest
    {
        private readonly RatingService _service;
        private readonly Mock<IRatingRepository> _repository;

        public RatingServiceDeleteRatingTest()
        {
            _repository = new Mock<IRatingRepository>();
            _service = new RatingService(_repository.Object);
        }

        [Fact]
        public void DeleteRatingById_WhenCalled_InvokesRepositoryDeleteOnce()
        {
            // Arrange
            int ratingId = 42;

            // Act
            _service.DeleteRatingById(ratingId);

            // Assert
            _repository.Verify(repo => repo.DeleteRating(ratingId), Times.Once);
        }
    }
}
