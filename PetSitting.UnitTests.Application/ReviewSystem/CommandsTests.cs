using Moq;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.ReviewSystem.Commands;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.ReviewSystem;

namespace PetSitting.UnitTests.Application.ReviewSystem
{
    public class CommandsTests
    {
        public CommandsTests()
        {
            
        }

        [Fact]
        public async Task HandleCommand_ShouldThrowError_IfValidationFails()
        {
            //arrange
            var _mockReviewRepository = new Mock<IReviewRepository>();
            var _mockReview = new Mock<Review>();
            _mockReviewRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(_mockReview.Object)!);
            _mockReviewRepository.Setup(r => r.Delete(It.IsAny<Review>()))
                .Returns(Task.CompletedTask);

            var command = new DeleteReviewCommand("");
            var commandHandler = new DeleteReviewCommandHandler(_mockReviewRepository.Object);
            
            //act & assert
            await Assert.ThrowsAsync<GenericValidationException>(() => commandHandler.Handle(command,CancellationToken.None));
            _mockReviewRepository.Verify(r => r.GetByIdAsync(It.IsAny<string>()), Times.Never);
            _mockReviewRepository.Verify(r => r.Delete(It.IsAny<Review>()), Times.Never);
                
        }


        [Fact]
        public async Task HandleCommand_ShouldThrowError_IfReviewDoesNotExists()
        {
            //arrange
            var _mockReviewRepository = new Mock<IReviewRepository>();
            var _mockReview = new Mock<Review>();
            _mockReviewRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult((Review?)null));
            _mockReviewRepository.Setup(r => r.Delete(It.IsAny<Review>()))
                .Returns(Task.CompletedTask);

            var command = new DeleteReviewCommand("tests");
            var commandHandler = new DeleteReviewCommandHandler(_mockReviewRepository.Object);
            
            //act & assert
            await Assert.ThrowsAsync<ReviewNotFoundException>(() => commandHandler.Handle(command,CancellationToken.None));
            _mockReviewRepository.Verify(r => r.GetByIdAsync(It.IsAny<string>()), Times.Once);
            _mockReviewRepository.Verify(r => r.Delete(It.IsAny<Review>()), Times.Never);
                
        }

        [Fact]
        public async Task HandleCommand_ShouldDeleteReview_IfChecksSucceeds()
        {
            //arrange
            var _mockReviewRepository = new Mock<IReviewRepository>();
            var _mockReview = new Mock<Review>();
            _mockReviewRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(_mockReview.Object)!);
            _mockReviewRepository.Setup(r => r.Delete(It.IsAny<Review>()))
                .Returns(Task.CompletedTask);

            var command = new DeleteReviewCommand("test");
            var commandHandler = new DeleteReviewCommandHandler(_mockReviewRepository.Object);
            
            //act
            await commandHandler.Handle(command,CancellationToken.None);

            //assert
            _mockReviewRepository.Verify(r => r.GetByIdAsync(It.IsAny<string>()), Times.Once);
            _mockReviewRepository.Verify(r => r.Delete(It.IsAny<Review>()), Times.Once);
                
        }

        [Fact]
        public async Task HandleCommand_ShouldNotLeaveReview_IfValidationFails()
        {
            //arrange
            var _mockReviewRepository = new Mock<IReviewRepository>();
            var _mockReviews = new Mock<ICollection<Review>>();
            var _mockReview = new Mock<Review>();
            _mockReviewRepository.Setup(r => r.GetReviewsByAuthorToPost(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(_mockReviews.Object)!);
            _mockReviewRepository.Setup(r => r.AddAsync(It.IsAny<Review>()))
                .Returns(Task.FromResult(_mockReview.Object));

            var command = new LeaveReviewCommand("","",0,"","");
            var commandHandler = new LeaveReviewCommandHandler(_mockReviewRepository.Object);
            
            //act
            await commandHandler.Handle(command,CancellationToken.None);

            //assert
            _mockReviewRepository.Verify(r => r.GetReviewsByAuthorToPost(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _mockReviewRepository.Verify(r => r.AddAsync(It.IsAny<Review>()), Times.Never);
                
        }

        [Fact]
        public async Task HandleCommand_ShouldNotLeaveReview_IfUserAlreadyLeftAReview()
        {
            //arrange
            var _mockReviewRepository = new Mock<IReviewRepository>();
            var _mockReviews = new List<Review>();
            _mockReviews = new List<Review> {
                new Review {Title = "tets", Content = "Test", Rating = 2, AuthorId = "test", PostId = "Test"},
                new Review {Title = "tetss", Content = "Teast", Rating = 3, AuthorId = "tesat", PostId = "Taest"}
            };
            var _mockReview = new Mock<Review>();
            _mockReviewRepository.Setup(r => r.GetReviewsByAuthorToPost(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(_mockReviews);
            _mockReviewRepository.Setup(r => r.AddAsync(It.IsAny<Review>()))
                .Returns(Task.FromResult(_mockReview.Object));

            var command = new LeaveReviewCommand("test","test",2,"test","test");
            var commandHandler = new LeaveReviewCommandHandler(_mockReviewRepository.Object);
            
            //act & assert
            await Assert.ThrowsAsync<CannotReviewException>(() => commandHandler.Handle(command,CancellationToken.None));

            //assert
            _mockReviewRepository.Verify(r => r.GetReviewsByAuthorToPost(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _mockReviewRepository.Verify(r => r.AddAsync(It.IsAny<Review>()), Times.Never);
                
        }

        [Fact]
        public async Task HandleCommand_ShouldLeaveReview_IfChecksSucceeds()
        {
            //arrange
            var _mockReviewRepository = new Mock<IReviewRepository>();
            var _mockReviews = new Mock<ICollection<Review>>();
            var _mockReview = new Mock<Review>();
            _mockReviewRepository.Setup(r => r.GetReviewsByAuthorToPost(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(_mockReviews.Object)!);
            _mockReviewRepository.Setup(r => r.AddAsync(It.IsAny<Review>()))
                .Returns(Task.FromResult(_mockReview.Object));

            var command = new LeaveReviewCommand("test","test",2,"test","test");
            var commandHandler = new LeaveReviewCommandHandler(_mockReviewRepository.Object);
            
            //act
            await commandHandler.Handle(command,CancellationToken.None);

            //assert
            _mockReviewRepository.Verify(r => r.GetReviewsByAuthorToPost(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _mockReviewRepository.Verify(r => r.AddAsync(It.IsAny<Review>()), Times.Once);
                
        }

        [Fact]
        public async Task HandleCommand_ShouldNotUpdateReview_IfValidationFails()
        {
            //arrange
            var _mockReviewRepository = new Mock<IReviewRepository>();
            var _mockReviews = new Mock<ICollection<Review>>();
            var _mockReview = new Mock<Review>();
            _mockReviewRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(_mockReview.Object)!);
            _mockReviewRepository.Setup(r => r.UpdateAsync(It.IsAny<Review>()))
                .Returns(Task.FromResult(_mockReview.Object));

            var command = new UpdateReviewCommand("","","","","",0);
            var commandHandler = new UpdateReviewCommandHandler(_mockReviewRepository.Object);

            //act & assert
            await Assert.ThrowsAsync<GenericValidationException>(() => commandHandler.Handle(command,CancellationToken.None));
            _mockReviewRepository.Verify(r => r.GetByIdAsync(It.IsAny<string>()), Times.Never);
            _mockReviewRepository.Verify(r => r.UpdateAsync(It.IsAny<Review>()), Times.Never);
        }

        [Fact]
        public async Task HandleCommand_ShouldNotUpdateReview_IfReviewDoesNotExists()
        {
            //arrange
            var _mockReviewRepository = new Mock<IReviewRepository>();
            var _mockReviews = new Mock<ICollection<Review>>();
            var _mockReview = new Mock<Review>();
            _mockReviewRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((Review?)null);
            _mockReviewRepository.Setup(r => r.UpdateAsync(It.IsAny<Review>()))
                .Returns(Task.FromResult(_mockReview.Object));

            var command = new UpdateReviewCommand("test","test","test","test","test",2);
            var commandHandler = new UpdateReviewCommandHandler(_mockReviewRepository.Object);

            //act & assert
            await Assert.ThrowsAsync<ReviewNotFoundException>(() => commandHandler.Handle(command,CancellationToken.None));
            _mockReviewRepository.Verify(r => r.GetByIdAsync(It.IsAny<string>()), Times.Once);
            _mockReviewRepository.Verify(r => r.UpdateAsync(It.IsAny<Review>()), Times.Never);
        }

        [Fact]
        public async Task HandleCommand_ShouldNotUpdateReview_IfItWasAlreadyUpdatedTwice()
        {
            //arrange
            var _mockReviewRepository = new Mock<IReviewRepository>();
            var _mockReviews = new Mock<ICollection<Review>>();
            var _mockReview = new Mock<Review>();
            _mockReview.Object.UpdatedCount = 2;
            _mockReviewRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_mockReview.Object);
            _mockReviewRepository.Setup(r => r.UpdateAsync(It.IsAny<Review>()))
                .Returns(Task.FromResult(_mockReview.Object));

            var command = new UpdateReviewCommand("test","test","test","test","test",2);
            var commandHandler = new UpdateReviewCommandHandler(_mockReviewRepository.Object);

            //act & assert
            await Assert.ThrowsAsync<CannotUpdateException>(() => commandHandler.Handle(command,CancellationToken.None));
            _mockReviewRepository.Verify(r => r.GetByIdAsync(It.IsAny<string>()), Times.Once);
            _mockReviewRepository.Verify(r => r.UpdateAsync(It.IsAny<Review>()), Times.Never);
        }

        [Fact]
        public async Task HandleCommand_ShouldUpdateReview_IfChecksSucceeds()
        {
            //arrange
            var _mockReviewRepository = new Mock<IReviewRepository>();
            var _mockReviews = new Mock<ICollection<Review>>();
            var _mockReview = new Mock<Review>();
            _mockReview.Object.UpdatedCount = 1;
            _mockReviewRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_mockReview.Object);
            _mockReviewRepository.Setup(r => r.UpdateAsync(It.IsAny<Review>()))
                .Returns(Task.FromResult(_mockReview.Object));

            var command = new UpdateReviewCommand("test","test","test","test","test",2);
            var commandHandler = new UpdateReviewCommandHandler(_mockReviewRepository.Object);

            //act & assert
            await commandHandler.Handle(command,CancellationToken.None);
            _mockReviewRepository.Verify(r => r.GetByIdAsync(It.IsAny<string>()), Times.Once);
            _mockReviewRepository.Verify(r => r.UpdateAsync(It.IsAny<Review>()), Times.Once);
        }
    }
}