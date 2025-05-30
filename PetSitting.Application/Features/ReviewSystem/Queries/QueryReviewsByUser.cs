using MediatR;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.ReviewSystem;

namespace PetSitting.Application.Features.ReviewSystem.Queries
{
    public record QueryReviewsByUser(string userId) : IRequest<QueryReviewsByUserResponse>;
    public record QueryReviewsByUserResponse : BaseResponse
    {
        public ICollection<Review>? Reviews {get;set;}
    }

    public class QueryReviewsByUserHandler : BaseHandler<QueryReviewsByUser, QueryReviewsByUserResponse>
    {
        private readonly IReviewRepository _reviewRepository;
        public QueryReviewsByUserHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        protected override async Task<QueryReviewsByUserResponse> HandleCommand(QueryReviewsByUser request, QueryReviewsByUserResponse response, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.userId))
                throw new GenericValidationException("Invalid parameters.");

            response.Reviews = await _reviewRepository.GetByAuthorIdAsync(request.userId);
            return response;
        }
    }
}