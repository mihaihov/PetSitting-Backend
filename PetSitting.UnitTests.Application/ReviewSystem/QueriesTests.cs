using Moq;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.ReviewSystem.Queries;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.ReviewSystem;

namespace PetSitting.UnitTests.Application.ReviewSystem.Queries
{
    public class QueriesTests
    {
        public QueriesTests()
        {
            
        }

        [Fact]
        public async Task HandleCommand_ShouldNotGetReviews_IfValidationFails()
        {
            //arrange
            var _mockReviewRepository = new Mock<IReviewRepository>();
            var _mockReview = new Mock<Review>();
            var _mockReviews = new Mock<ICollection<Review>>();
            _mockReviewRepository.Setup(r => r.GetByPostIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_mockReviews.Object);

            var command = new GetReviewsByPostQuery("");
            var commandHandler = new GetReviewsByPostQueryHandler(_mockReviewRepository.Object);

            //act&assert
            await Assert.ThrowsAsync<GenericValidationException>(() => commandHandler.Handle(command,CancellationToken.None));
            _mockReviewRepository.Verify(r => r.GetByPostIdAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task HandleCommand_ShouldGetReviews_IfValidationSucceeds()
        {
            //arrange
            var _mockReviewRepository = new Mock<IReviewRepository>();
            var _mockReview = new Mock<Review>();
            var _mockReviews = new Mock<ICollection<Review>>();
            _mockReviewRepository.Setup(r => r.GetByPostIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_mockReviews.Object);

            var command = new GetReviewsByPostQuery("test");
            var commandHandler = new GetReviewsByPostQueryHandler(_mockReviewRepository.Object);

            //act&assert
            await commandHandler.Handle(command,CancellationToken.None);
            _mockReviewRepository.Verify(r => r.GetByPostIdAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task HandleCommand_ShouldNotGetReviewsByUser_IfValidationFails()
        {
            //arrange
            var _mockReviewRepository = new Mock<IReviewRepository>();
            var _mockReview = new Mock<Review>();
            var _mockReviews = new Mock<ICollection<Review>>();
            _mockReviewRepository.Setup(r => r.GetByAuthorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_mockReviews.Object);

            var command = new GetReviewsByUserQuery("");
            var commandHandler = new GetReviewsByUserQueryHandler(_mockReviewRepository.Object);

            //act&assert
            await Assert.ThrowsAsync<GenericValidationException>(() => commandHandler.Handle(command,CancellationToken.None));
            _mockReviewRepository.Verify(r => r.GetByPostIdAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task HandleCommand_ShouldGetReviewsByUser_IfValidationSucceeds()
        {
            //arrange
            var _mockReviewRepository = new Mock<IReviewRepository>();
            var _mockReview = new Mock<Review>();
            var _mockReviews = new Mock<ICollection<Review>>();
            _mockReviewRepository.Setup(r => r.GetByAuthorIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_mockReviews.Object);

            var command = new GetReviewsByUserQuery("test");
            var commandHandler = new GetReviewsByUserQueryHandler(_mockReviewRepository.Object);

            //act&assert
            await commandHandler.Handle(command,CancellationToken.None);
            _mockReviewRepository.Verify(r => r.GetByAuthorIdAsync(It.IsAny<string>()), Times.Once);
        }
    }
}