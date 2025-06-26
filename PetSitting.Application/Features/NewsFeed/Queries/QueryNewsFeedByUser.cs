using MediatR;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.NewsFeed;
using PetSitting.Domain.Entities.PostManagement;

namespace PetSitting.Application.Features.NewsFeed.Queries
{
    public record QueryNewsFeedByUser(string userId) : IRequest<QueryNewsFeedByUserResponse>;
    public record QueryNewsFeedByUserResponse : BaseResponse
    {
        public List<Post>? Posts { get; set; }
    }
    public class QueryNewsFeedByUserHandler : BaseHandler<QueryNewsFeedByUser, QueryNewsFeedByUserResponse>
    {
        private readonly INewsFeedRepository _newsFeedRepository;
        private readonly IBaseRepository<Post> _postRepository;
        public QueryNewsFeedByUserHandler(INewsFeedRepository newsFeedRepository, IBaseRepository<Post> postRepository)
        {
            _newsFeedRepository = newsFeedRepository;
            _postRepository = postRepository;
        }
        protected override async Task<QueryNewsFeedByUserResponse> HandleCommand(QueryNewsFeedByUser request, QueryNewsFeedByUserResponse response, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.userId))
                throw new GenericValidationException("Invalid data.");

            var feed = await _newsFeedRepository.GetFeedByUserAsync(request.userId);
            response.Posts = new List<Post>();

            var posts = feed?.Select(feed => feed.PostId);
            if (posts != null)
            {
                foreach (var postId in posts)
                {
                    var post = await _postRepository.GetByIdAsync(postId);
                    if(post != null)
                        response.Posts.Add(post);
                }
            }

            return response;
        }
    }
}