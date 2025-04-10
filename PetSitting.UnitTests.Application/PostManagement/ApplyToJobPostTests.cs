using Moq;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.PostManagement.Commands;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.PostManagement;
using PetSitting.Domain.Entities.UserManagement;

namespace PetSitting.UnitTests.Application.PostManagement
{
    public class ApplyToJobPostTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IBaseRepository<Post>> _mockPostsRepository;
        private readonly Mock<IJobApplicationRepository> _mockJobApplicationRepository;
        private readonly Mock<JobApplication> _mockJobApplication;
        private readonly Mock<ApplicationUser> _mockApplicationUser;
        private readonly Mock<Post> _mockPost;
        public ApplyToJobPostTests()
        {
            _mockPost = new Mock<Post>();
            _mockJobApplication = new Mock<JobApplication>();
            _mockApplicationUser = new Mock<ApplicationUser>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPostsRepository = new Mock<IBaseRepository<Post>>();
            _mockJobApplicationRepository = new Mock<IJobApplicationRepository>();
            _mockJobApplicationRepository.Setup(j => j.AddAsync(It.IsAny<JobApplication>()))
                .Returns(Task.FromResult(_mockJobApplication.Object));
        }

        [Fact]
        public async Task HandleCommand_ShouldNotApply_IfValidationFails()
        {
            //arrange
            var command = new ApplyToJobPostCommand("","","","");

            //act
            var commandHandler = new ApplyToJobPostCommandHandler(_mockUserRepository.Object, 
                _mockPostsRepository.Object, _mockJobApplicationRepository.Object);
            var response = await commandHandler.Handle(command,CancellationToken.None);

            //assert
            _mockUserRepository.Verify(u => u.GetByIdAsync(It.IsAny<string>()), Times.Never);
            _mockPostsRepository.Verify(p => p.GetByIdAsync(It.IsAny<string>()), Times.Never);
            _mockJobApplicationRepository.Verify(j => j.Exists(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _mockJobApplicationRepository.Verify(j => j.AddAsync(It.IsAny<JobApplication>()), Times.Never);
            Assert.False(response.Success);
            Assert.NotNull(response.ValidationErrors);
            Assert.NotEmpty(response.ValidationErrors);
        }

        [Fact]
        public async Task HandleCommand_ShouldNotApply_IfApplicantIsNotFound()
        {
            //arrange
            var command = new ApplyToJobPostCommand("test1","test2","test3","test4");
            _mockUserRepository.Setup(u => u.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser?)null);

            //act
            var commandHandler = new ApplyToJobPostCommandHandler(_mockUserRepository.Object, 
                _mockPostsRepository.Object, _mockJobApplicationRepository.Object);
            await Assert.ThrowsAsync<Exception>(() => commandHandler.Handle(command,CancellationToken.None));

            //assert
            _mockUserRepository.Verify(u => u.GetByIdAsync(It.IsAny<string>()), Times.Once);
            _mockPostsRepository.Verify(p => p.GetByIdAsync(It.IsAny<string>()), Times.Never);
            _mockJobApplicationRepository.Verify(j => j.Exists(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _mockJobApplicationRepository.Verify(j => j.AddAsync(It.IsAny<JobApplication>()), Times.Never);
        }

        [Fact]
        public async Task HandleCommand_ShouldNotApply_IfPostIsNotFound()
        {
            //arrange
            var command = new ApplyToJobPostCommand("test1","test2","test3","test4");
            _mockUserRepository.Setup(u => u.GetByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(_mockApplicationUser.Object)!);

            _mockPostsRepository.Setup(u => u.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((JobPost?)null);

            //act
            var commandHandler = new ApplyToJobPostCommandHandler(_mockUserRepository.Object, 
                _mockPostsRepository.Object, _mockJobApplicationRepository.Object);
            await Assert.ThrowsAsync<Exception>(() => commandHandler.Handle(command,CancellationToken.None));

            //assert
            _mockUserRepository.Verify(u => u.GetByIdAsync(It.IsAny<string>()), Times.Once);
            _mockPostsRepository.Verify(p => p.GetByIdAsync(It.IsAny<string>()), Times.Once);
            _mockJobApplicationRepository.Verify(j => j.Exists(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _mockJobApplicationRepository.Verify(j => j.AddAsync(It.IsAny<JobApplication>()), Times.Never);
        }

        [Fact]
        public async Task HandleCommand_ShouldNotApply_IfAnApplicationWasAlreadySubmitted()
        {
            //arrange
            var command = new ApplyToJobPostCommand("test1","test2","test3","test4");
            _mockUserRepository.Setup(u => u.GetByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(_mockApplicationUser.Object)!);

            _mockPostsRepository.Setup(u => u.GetByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(_mockPost.Object)!);

            _mockJobApplicationRepository.Setup(ja => ja.Exists(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            //act
            var commandHandler = new ApplyToJobPostCommandHandler(_mockUserRepository.Object, 
                _mockPostsRepository.Object, _mockJobApplicationRepository.Object);
            await Assert.ThrowsAsync<Exception>(() => commandHandler.Handle(command,CancellationToken.None));

            //assert
            _mockUserRepository.Verify(u => u.GetByIdAsync(It.IsAny<string>()), Times.Once);
            _mockPostsRepository.Verify(p => p.GetByIdAsync(It.IsAny<string>()), Times.Once);
            _mockJobApplicationRepository.Verify(j => j.Exists(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _mockJobApplicationRepository.Verify(j => j.AddAsync(It.IsAny<JobApplication>()), Times.Never);
        }

        [Fact]
        public async Task HandleCommand_ShouldApply_IfEverythingsSucceeds()
        {
            //arrange
            var command = new ApplyToJobPostCommand("test1","test2","test3","test4");
            _mockUserRepository.Setup(u => u.GetByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(_mockApplicationUser.Object)!);

            _mockPostsRepository.Setup(u => u.GetByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(_mockPost.Object)!);

            _mockJobApplicationRepository.Setup(ja => ja.Exists(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            //act
            var commandHandler = new ApplyToJobPostCommandHandler(_mockUserRepository.Object, 
                _mockPostsRepository.Object, _mockJobApplicationRepository.Object);
            await commandHandler.Handle(command,CancellationToken.None);

            //assert
            _mockUserRepository.Verify(u => u.GetByIdAsync(It.IsAny<string>()), Times.Once);
            _mockPostsRepository.Verify(p => p.GetByIdAsync(It.IsAny<string>()), Times.Once);
            _mockJobApplicationRepository.Verify(j => j.Exists(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _mockJobApplicationRepository.Verify(j => j.AddAsync(It.IsAny<JobApplication>()), Times.Once);
        }
    }
}