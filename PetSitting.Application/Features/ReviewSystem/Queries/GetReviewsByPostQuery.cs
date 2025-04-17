using MediatR;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.ReviewSystem;

namespace PetSitting.Application.Features.ReviewSystem.Queries
{
    public record GetReviewsByPostQuery(string postId) : IRequest<GetReviewsByPostQueryResponse>;
    public record GetReviewsByPostQueryResponse : BaseResponse
    {
        public ICollection<Review>? Reviews {get;set;}
    }

    public class GetReviewsByPostQueryHandler : BaseHandler<GetReviewsByPostQuery, GetReviewsByPostQueryResponse>
    {
        private readonly IReviewRepository _reviewRepository;
        public GetReviewsByPostQueryHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        protected override async Task<GetReviewsByPostQueryResponse> HandleCommand(GetReviewsByPostQuery request, GetReviewsByPostQueryResponse response, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.postId))
                throw new GenericValidationException("Invalid parameters.");

            response.Reviews = await _reviewRepository.GetByPostIdAsync(request.postId);
            return response;
        }
    }
}