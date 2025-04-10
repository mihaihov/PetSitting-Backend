using Moq;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.PostManagement.Commands;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.PostManagement;

namespace PetSitting.UnitTests.Application.PostManagement
{
    public class CrudTests
    {
        private readonly Mock<IBaseRepository<JobPost>> _mockJobPostRepository;
        private readonly Mock<IJobApplicationRepository> _mockJobApplicationRepository;
        private readonly Mock<JobPost> _mockJobPost;
        private readonly Mock<JobApplication> _mockJobApplication;
        public CrudTests()
        {
            _mockJobPost = new Mock<JobPost>();
            _mockJobPostRepository = new Mock<IBaseRepository<JobPost>>();
            _mockJobApplicationRepository = new Mock<IJobApplicationRepository>();
            _mockJobApplication = new Mock<JobApplication>();

            _mockJobPostRepository.Setup(j => j.AddAsync(It.IsAny<JobPost>()))
                .Returns(Task.FromResult(_mockJobPost.Object));
            _mockJobPostRepository.Setup(j => j.Update(It.IsAny<JobPost>()))
                .Returns(Task.FromResult(_mockJobPost.Object));
            _mockJobPostRepository.Setup(j => j.Delete(It.IsAny<JobPost>()))
                .Returns(Task.FromResult(_mockJobPost.Object));
                
        }
        [Fact]
        public async Task HandleCommand_ShouldCreatePost_WhenValidationSucceeds()
        {
            //arrange
            var command = new CreateJobPostCommand(description: "Test description",
                                                                authorId: "testauthor",
                                                                medias: null,
                                                                title: "test title",
                                                                location: "test location",
                                                                startDate: DateTime.Now,
                                                                endDate: DateTime.Now,
                                                                payment: 99.99m);
            //act
            var commandHandler = new CreateJobPostCommandHandler(_mockJobPostRepository.Object);
            await commandHandler.Handle(command, CancellationToken.None);

            //assert
            _mockJobPostRepository.Verify(j => j.AddAsync(It.IsAny<JobPost>()),Times.Once);
        }

        [Fact]
        public async Task HandleCommand_ShouldNotCreatePost_WhenValidationFails()
        {
            //arrange
            var command = new CreateJobPostCommand(description: "Test description",
                                                    authorId: "",
                                                    medias: null,
                                                    title: "test title",
                                                    location: "test location",
                                                    startDate: DateTime.Now,
                                                    endDate: DateTime.Now,
                                                    payment: 99.99m);

            //act
            var commandHandler = new CreateJobPostCommandHandler(_mockJobPostRepository.Object);
            var response = await commandHandler.Handle(command,CancellationToken.None);

            //assert
            _mockJobPostRepository.Verify(j => j.AddAsync(It.IsAny<JobPost>()),Times.Never);
            Assert.False(response.Success);
            Assert.NotNull(response.ValidationErrors);
            Assert.NotEmpty(response.ValidationErrors);
        }

        [Fact]
        public async Task HandleCommand_ShouldNotDeletePost_IfValidationFails()
        {
            //arrange
            var command = new DeleteJobPostCommand("");

            //act
            var commandHandler = new DeleteJobPostCommandHandler(_mockJobPostRepository.Object);
            await Assert.ThrowsAsync<Exception>(() => commandHandler.Handle(command,CancellationToken.None));

            //assert
            _mockJobPostRepository.Verify(j => j.Delete(It.IsAny<JobPost>()),Times.Never);
        }

        [Fact]
        public async Task HandleCommand_ShouldNotDeletePost_IfJobPostDoesNotExists()
        {
            //arrange
            var command = new DeleteJobPostCommand("testId");
            _mockJobPostRepository.Setup(j => j.GetByIdAsync(It.IsAny<string>()))   //mocks GetByIdAsync to return null.
                .ReturnsAsync((JobPost?)null);

            //act
            var commandHandler = new DeleteJobPostCommandHandler(_mockJobPostRepository.Object);
            await Assert.ThrowsAsync<Exception>(() => commandHandler.Handle(command,CancellationToken.None));

            //assert
            _mockJobPostRepository.Verify(j => j.Delete(It.IsAny<JobPost>()),Times.Never);
        }

        [Fact]
        public async Task HandleCommand_ShouldCreateUser_IfValidationSucceedsAndJobPostIsFound()
        {
            //arrange
            var command = new DeleteJobPostCommand("testId");
            _mockJobPostRepository.Setup(j => j.GetByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(_mockJobPost.Object)!);

            //act
            var commandHandler = new DeleteJobPostCommandHandler(_mockJobPostRepository.Object);
            await commandHandler.Handle(command,CancellationToken.None);

            //assert
            _mockJobPostRepository.Verify(j => j.Delete(It.IsAny<JobPost>()), Times.Once);
        }

        [Fact]
        public async Task HandleCommand_ShouldNotUpdateJobPost_IfJobPostDoesNotExists()
        {
            //arrange
            var command = new UpdateJobPostCommand(id: "testId",
                                                description: "testDescription",
                                                medias: null,
                                                title: null,
                                                location: null,
                                                startDate: null,
                                                endDate: null,
                                                payment: null,
                                                isOpen: null);
            _mockJobPostRepository.Setup(j => j.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((JobPost?)null);

            //act
            var commandHandler = new UpdatejobPostCommandHandler(_mockJobPostRepository.Object);
            await Assert.ThrowsAsync<Exception>(() => commandHandler.Handle(command,CancellationToken.None));

            //assert
            _mockJobPostRepository.Verify(j => j.Update(It.IsAny<JobPost>()), Times.Never);
        }

        [Fact]
        public async Task HandleCommand_ShouldNotUpdateJobPost_IfValidationFails()
        {
            //arrange
            var command = new UpdateJobPostCommand(id: "",
                                                description: "testDescription",
                                                medias: null,
                                                title: null,
                                                location: null,
                                                startDate: null,
                                                endDate: null,
                                                payment: null,
                                                isOpen: null);

            //act
            var commandHandler = new UpdatejobPostCommandHandler(_mockJobPostRepository.Object);
            var response = await commandHandler.Handle(command,CancellationToken.None);

            //assert
            _mockJobPostRepository.Verify(j => j.Update(It.IsAny<JobPost>()), Times.Never);
            Assert.False(response.Success);
            Assert.NotNull(response.ValidationErrors);
            Assert.NotEmpty(response.ValidationErrors);

        }

        [Fact]
        public async Task HandleCommand_ShouldUpdateJobPost_IfValidationSucceedsAndJobPostExists()
        {
            //arrange
            var command = new UpdateJobPostCommand(id: "testId",
                                                description: "testDescription",
                                                medias: null,
                                                title: null,
                                                location: null,
                                                startDate: null,
                                                endDate: null,
                                                payment: null,
                                                isOpen: null);
            _mockJobPostRepository.Setup(j => j.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_mockJobPost.Object);

            //act
            var commandHandler = new UpdatejobPostCommandHandler(_mockJobPostRepository.Object);
            var response = await commandHandler.Handle(command,CancellationToken.None);

            //assert
            _mockJobPostRepository.Verify(j => j.Update(It.IsAny<JobPost>()), Times.Once);
            Assert.False(!response.Success);
            Assert.Null(response.ValidationErrors);

        }

        [Fact]
        public async Task HandleCommand_ShouldNotUpdateJobApplication_IfValidationFails()
        {
            //arrange
            var command = new UpdateJobApplicationCommand("","");
            
            //act
            _mockJobApplicationRepository.Setup(ja => ja.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_mockJobApplication.Object);
            _mockJobApplicationRepository.Setup(ja => ja.Update(It.IsAny<JobApplication>()))
                .Returns(Task.CompletedTask);
            var commandHandler = new UpdateJobApplicationCommandHandler(_mockJobApplicationRepository.Object);
            
            //assert
            await Assert.ThrowsAsync<Exception>(() => commandHandler.Handle(command,CancellationToken.None));
            _mockJobApplicationRepository.Verify(ja => ja.GetByIdAsync(It.IsAny<string>()), Times.Never);
            _mockJobApplicationRepository.Verify(ja => ja.Update(It.IsAny<JobApplication>()), Times.Never);

        }

        [Fact]
        public async Task HandleCommand_ShouldNotUpdateJobApplication_IfJobApplicationIsNotFound()
        {
            //arrange
            var command = new UpdateJobApplicationCommand("test","test");
            
            //act
            _mockJobApplicationRepository.Setup(ja => ja.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((JobApplication?)null);
            _mockJobApplicationRepository.Setup(ja => ja.Update(It.IsAny<JobApplication>()))
                .Returns(Task.CompletedTask);
            var commandHandler = new UpdateJobApplicationCommandHandler(_mockJobApplicationRepository.Object);
            
            //assert
            await Assert.ThrowsAsync<Exception>(() => commandHandler.Handle(command,CancellationToken.None));
            _mockJobApplicationRepository.Verify(ja => ja.GetByIdAsync(It.IsAny<string>()), Times.Once);
            _mockJobApplicationRepository.Verify(ja => ja.Update(It.IsAny<JobApplication>()), Times.Never);

        }

        [Fact]
        public async Task HandleCommand_ShouldUpdateJobApplication_IfEverythingIsOk()
        {
            //arrange
            var command = new UpdateJobApplicationCommand("test","test");
            
            //act
            _mockJobApplicationRepository.Setup(ja => ja.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_mockJobApplication.Object);
            _mockJobApplicationRepository.Setup(ja => ja.Update(It.IsAny<JobApplication>()))
                .Returns(Task.CompletedTask);
            var commandHandler = new UpdateJobApplicationCommandHandler(_mockJobApplicationRepository.Object);
            
            //assert
            await commandHandler.Handle(command,CancellationToken.None);
            _mockJobApplicationRepository.Verify(ja => ja.GetByIdAsync(It.IsAny<string>()), Times.Once);
            _mockJobApplicationRepository.Verify(ja => ja.Update(It.IsAny<JobApplication>()), Times.Once);

        }
    }
}