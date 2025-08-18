using Moq;
using PetSitting.Application.Features.NewsFeed.Queries;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.PostManagement;
using PetSitting.Domain.Entities.NewsFeed;

namespace PetSitting.UnitTests.Application.NewsFeedSystem
{
    public class QueriesTests
    {
        private class TestableQueryNewsFeedByUserHandler : QueryNewsFeedByUserHandler
        {
            public TestableQueryNewsFeedByUserHandler(INewsFeedRepository newsFeedRepo, IBaseRepository<JobPost> postRepo)
                : base(newsFeedRepo, postRepo) { }

            public Task<QueryNewsFeedByUserResponse> InvokeHandleCommand(QueryNewsFeedByUser request)
            {
                return HandleCommand(request, new QueryNewsFeedByUserResponse(), CancellationToken.None);
            }
        }

        [Fact]
        public async Task HandleCommand_NullRequest_ThrowsNullReferenceException()
        {
            var newsFeedRepo = new Mock<INewsFeedRepository>();
            var postRepo = new Mock<IBaseRepository<JobPost>>();
            var handler = new TestableQueryNewsFeedByUserHandler(newsFeedRepo.Object, postRepo.Object);

            await Assert.ThrowsAsync<NullReferenceException>(() => handler.InvokeHandleCommand(null!));
        }

        [Fact]
        public async Task HandleCommand_EmptyUserId_ThrowsGenericValidationException()
        {
            var newsFeedRepo = new Mock<INewsFeedRepository>();
            var postRepo = new Mock<IBaseRepository<JobPost>>();
            var handler = new TestableQueryNewsFeedByUserHandler(newsFeedRepo.Object, postRepo.Object);

            var request = new QueryNewsFeedByUser(string.Empty);

            await Assert.ThrowsAsync<GenericValidationException>(() => handler.InvokeHandleCommand(request));
        }

        [Fact]
        public async Task HandleCommand_FeedIsNull_ReturnsEmptyPostsList()
        {
            var newsFeedRepo = new Mock<INewsFeedRepository>();
            newsFeedRepo.Setup(r => r.GetFeedByUserAsync(It.IsAny<string>())).ReturnsAsync((IReadOnlyList<Feed>?)null);
            var postRepo = new Mock<IBaseRepository<JobPost>>();
            var handler = new TestableQueryNewsFeedByUserHandler(newsFeedRepo.Object, postRepo.Object);

            var request = new QueryNewsFeedByUser("user-1");

            var response = await handler.InvokeHandleCommand(request);

            Assert.NotNull(response);
            Assert.NotNull(response.Posts);
            Assert.Empty(response.Posts);
        }
    }
}