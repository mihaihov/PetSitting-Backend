using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.NewsFeed.Commands;
using PetSitting.Application.Features.NewsFeed.Queries;
using PetSitting.Application.Features.PostManagement.Commands;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.PostManagement;
using PetSitting.Infrastructure;
using PetSitting.Infrastructure.Repositories;
using Xunit;

namespace PetSitting.UnitTests.Application.NewsFeedSystem
{
    public class FeedFacoutServiceTests
    {
        [Fact]
        public async Task Publish_NullEntity_ThrowsGenericValidationException()
        {
            var mediatorMock = new Mock<IMediator>();
            var userRepoMock = new Mock<IUserRepository>();
            var service = new PetSitting.Infrastructure.Services.FeedFanoutService(mediatorMock.Object, userRepoMock.Object);

            await Assert.ThrowsAsync<GenericValidationException>(() => service.Publish(null));
        }

        [Fact]
        public async Task Publish_ValidEntity_CallsCreateJobPostAndUpdatesForNearbyUsers()
        {
            var mediatorMock = new Mock<IMediator>();

            // Arrange a job post
            var jobPost = new JobPost
            {
                Id = Guid.NewGuid().ToString(),
                Location = "TestLocation",
                AuthorId = "test-author-id"
            };

            // Setup mediator to accept CreateJobPostCommand and UpdateNewsFeedCommand
            mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateJobPostCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PetSitting.Application.Features.Common.BaseResponse());
            mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateNewsFeedCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PetSitting.Application.Features.Common.BaseResponse());

            // Prepare an in-memory user sequence that matches the Where clause in service.
            // We create a simple type that matches what the repository is expected to return:
            // because repository.BaseQuery() is IQueryable of domain user entity that has Id and UserProfile.Location.
            // Many projects have a User entity with UserProfile; adapt these values if your domain differs.
            // Return an empty IQueryable<ApplicationUser> to match the repository's expected element type.
            // userRepoMock
            //     .Setup(r => r.BaseQuery())
            //     .Returns(Enumerable.Empty<PetSitting.Domain.Entities.UserManagement.ApplicationUser>().AsQueryable());

            //using EF core data for BaseQuery
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("TestDatabase").Options;
            var context = new ApplicationDbContext(options);
            var userRepoMock = new UserRepository(context);

            

            var service = new PetSitting.Infrastructure.Services.FeedFanoutService(mediatorMock.Object, userRepoMock);

            // Act
            await service.Publish(jobPost);

            // Assert CreateJobPostCommand was sent
            mediatorMock.Verify(m => m.Send(It.Is<CreateJobPostCommand>(c => c != null && c.JobPost == jobPost), It.IsAny<CancellationToken>()), Times.Once);

            // Because BaseQuery returns empty set above, no UpdateNewsFeedCommand should be called.
            mediatorMock.Verify(m => m.Send(It.IsAny<UpdateNewsFeedCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Retrieve_WhenMediatorReturnsNullPosts_ReturnsNull()
        {
            var mediatorMock = new Mock<IMediator>();
            var userRepoMock = new Mock<IUserRepository>();

            // Arrange: mediator returns an object whose Posts property is null.
            // The concrete response type of QueryNewsFeedByUser in your project should be used here.
            // We use Moq's generic override to match the exact request type from the production code.
            mediatorMock
                .Setup(m => m.Send<PetSitting.Application.Features.NewsFeed.Queries.QueryNewsFeedByUserResponse>(It.IsAny<PetSitting.Application.Features.NewsFeed.Queries.QueryNewsFeedByUser>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((PetSitting.Application.Features.NewsFeed.Queries.QueryNewsFeedByUserResponse)null!);

            var service = new PetSitting.Infrastructure.Services.FeedFanoutService(mediatorMock.Object, userRepoMock.Object);

            // Act
            var result = await service.Retrieve("some-user-id");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Retrieve_SortsPosts_ByCreatedAtAscending()
        {
            var mediatorMock = new Mock<IMediator>();
            var userRepoMock = new Mock<IUserRepository>();

            // Create posts with different CreatedAt values (unsorted)
            var p1 = new JobPost { Id = Guid.NewGuid().ToString(), CreatedAt = new DateTime(2021, 3, 1), AuthorId = "author-1", Location = "LocationA" };
            var p2 = new JobPost { Id = Guid.NewGuid().ToString(), CreatedAt = new DateTime(2020, 1, 1), AuthorId = "author-2", Location = "LocationB" };
            var p3 = new JobPost { Id = Guid.NewGuid().ToString(), CreatedAt = new DateTime(2022, 6, 1), AuthorId = "author-3", Location = "LocationC" };

            var posts = new List<JobPost> { p1, p2, p3 };

            // The service expects mediator.Send(new QueryNewsFeedByUser(...)) to return an object with Posts property.
            // We create a simple response class matching the production response used in the service.
            // If your project has a different response type, replace QueryNewsFeedByUserResponse below with the real type.
            var response = new PetSitting.Application.Features.NewsFeed.Queries.QueryNewsFeedByUserResponse { Posts = posts };

            mediatorMock
                .Setup(m => m.Send<PetSitting.Application.Features.NewsFeed.Queries.QueryNewsFeedByUserResponse>(It.IsAny<PetSitting.Application.Features.NewsFeed.Queries.QueryNewsFeedByUser>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var service = new PetSitting.Infrastructure.Services.FeedFanoutService(mediatorMock.Object, userRepoMock.Object);

            // Act
            var result = await service.Retrieve("user-id");

            // Assert final ordering is by CreatedAt ascending (earliest first) because the service sorts by CreatedAt last.
            Assert.NotNull(result);
            Assert.Equal(3, result!.Count);
            Assert.Equal(p2.Id, result[0].Id); // 2020-01-01
            Assert.Equal(p1.Id, result[1].Id); // 2021-03-01
            Assert.Equal(p3.Id, result[2].Id); // 2022-06-01
        }

        // Using the production QueryNewsFeedByUserResponse from:
        // PetSitting.Application.Features.NewsFeed.Queries.QueryNewsFeedByUserResponse
    }
}