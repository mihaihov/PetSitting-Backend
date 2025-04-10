using System.Net;
using Moq;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.PostManagement.Commands;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.PostManagement;

namespace PetSitting.UnitTests.Application.PostManagement
{
    public class UpdateJobOfferStatusTests
    {
        private readonly Mock<IJobApplicationRepository> _mockJobApplicationrepository;
        private readonly Mock<JobApplication> _mockJobApplication;
        private readonly Mock<IReadOnlyList<JobApplication>> _mockJobApplicationList;

        public UpdateJobOfferStatusTests()
        {
            _mockJobApplication = new Mock<JobApplication>();
            _mockJobApplicationList = new Mock<IReadOnlyList<JobApplication>>();
            _mockJobApplicationrepository = new Mock<IJobApplicationRepository>();
            _mockJobApplicationrepository.Setup(j => j.Update(It.IsAny<JobApplication>()))
                .Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task HandleCommand_ShouldNotUpdateStatus_IfValidationFails()
        {
            //arrange
            var command = new UpdateJobOfferStatusCommand("",Domain.Entities.PostManagement.JobApplicationStatus.Approved);

            //act
            var commandHandler = new UpdateJobOfferStatusCommandHandler(_mockJobApplicationrepository.Object);
            var respons = await commandHandler.Handle(command, CancellationToken.None);

            //assert
            Assert.False(respons.Success);
            Assert.NotNull(respons.ValidationErrors);
            Assert.NotEmpty(respons.ValidationErrors);
            _mockJobApplicationrepository.Verify(j => j.Update(It.IsAny<JobApplication>()), Times.Never);
        }

        [Fact]
        public async Task HandleCommand_ShouldNotUpdateStatus_IfJobApplicationDoesNotExists()
        {
            //arrange
            var command = new UpdateJobOfferStatusCommand("test1",Domain.Entities.PostManagement.JobApplicationStatus.Approved);
            _mockJobApplicationrepository.Setup(j => j.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((JobApplication?)null);
            //act
            var commandHandler = new UpdateJobOfferStatusCommandHandler(_mockJobApplicationrepository.Object);
            await Assert.ThrowsAsync<JobApplicationNotFoundException>(() => commandHandler.Handle(command, CancellationToken.None));

            //assert
            _mockJobApplicationrepository.Verify(j => j.Update(It.IsAny<JobApplication>()), Times.Never);
        }

        [Fact]
        public async Task HandleCommand_ShouldNotUpdateStatus_IfAJobOfferWasAlreadyAccepted()
        {
            //arrange
            var command = new UpdateJobOfferStatusCommand("test1",Domain.Entities.PostManagement.JobApplicationStatus.Approved);
            _mockJobApplicationrepository.Setup(j => j.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_mockJobApplication.Object);
            _mockJobApplicationrepository.Setup(j => j.GetAllJobApplicationsForAJobPost(It.IsAny<string>()))
                    .ReturnsAsync(new List<JobApplication>
                    {
                        new JobApplication { JobPostId = "TEST", ApplicantId = "test", Status = JobApplicationStatus.Pending, Description = "Test" },
                        new JobApplication { JobPostId = "TEST", ApplicantId = "test", Status = JobApplicationStatus.Approved, Description = "Test" }, // This will be matched by LINQ
                    });;
            //act
            var commandHandler = new UpdateJobOfferStatusCommandHandler(_mockJobApplicationrepository.Object);
            await Assert.ThrowsAsync<JobApplicationAlreadyAcceptedException>(() => commandHandler.Handle(command, CancellationToken.None));

            //assert
            _mockJobApplicationrepository.Verify(j => j.Update(It.IsAny<JobApplication>()), Times.Never);
        }

                [Fact]
        public async Task HandleCommand_ShouldUpdateStatus_IfEverythingIsValid()
        {
            //arrange
            var command = new UpdateJobOfferStatusCommand("test1",Domain.Entities.PostManagement.JobApplicationStatus.Approved);
            _mockJobApplicationrepository.Setup(j => j.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_mockJobApplication.Object);
            _mockJobApplicationrepository.Setup(j => j.GetAllJobApplicationsForAJobPost(It.IsAny<string>()))
                    .ReturnsAsync(new List<JobApplication>
                    {
                        new JobApplication { JobPostId = "TEST", ApplicantId = "test", Status = JobApplicationStatus.Pending, Description = "test" },
                        new JobApplication { JobPostId = "TEST", ApplicantId = "test", Status = JobApplicationStatus.Pending, Description = "test" }, // This will be matched by LINQ
                    });;
            //act
            var commandHandler = new UpdateJobOfferStatusCommandHandler(_mockJobApplicationrepository.Object);
            await commandHandler.Handle(command, CancellationToken.None);

            //assert
            _mockJobApplicationrepository.Verify(j => j.Update(It.IsAny<JobApplication>()), Times.Once);
        }
    }
}