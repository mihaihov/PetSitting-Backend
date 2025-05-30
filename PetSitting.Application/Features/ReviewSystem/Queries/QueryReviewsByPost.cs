using MediatR;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.ReviewSystem;

namespace PetSitting.Application.Features.ReviewSystem.Queries
{
    public record QueryReviewsByPost(string postId) : IRequest<QueryReviewsByPostResponse>;
    public record QueryReviewsByPostResponse : BaseResponse
    {
        public ICollection<Review>? Reviews {get;set;}
    }

    public class QueryReviewsByPostHandler : BaseHandler<QueryReviewsByPost, QueryReviewsByPostResponse>
    {
        private readonly IReviewRepository _reviewRepository;
        public QueryReviewsByPostHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        protected override async Task<QueryReviewsByPostResponse> HandleCommand(QueryReviewsByPost request, QueryReviewsByPostResponse response, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.postId))
                throw new GenericValidationException("Invalid parameters.");

            response.Reviews = await _reviewRepository.GetByPostIdAsync(request.postId);
            return response;
        }
    }
}