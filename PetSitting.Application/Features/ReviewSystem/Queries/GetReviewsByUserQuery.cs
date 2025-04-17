using MediatR;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.ReviewSystem;

namespace PetSitting.Application.Features.ReviewSystem.Queries
{
    public record GetReviewsByUserQuery(string userId) : IRequest<GetReviewsByUserQueryResponse>;
    public record GetReviewsByUserQueryResponse : BaseResponse
    {
        public ICollection<Review>? Reviews {get;set;}
    }

    public class GetReviewsByUserQueryHandler : BaseHandler<GetReviewsByUserQuery, GetReviewsByUserQueryResponse>
    {
        private readonly IReviewRepository _reviewRepository;
        public GetReviewsByUserQueryHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        protected override async Task<GetReviewsByUserQueryResponse> HandleCommand(GetReviewsByUserQuery request, GetReviewsByUserQueryResponse response, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.userId))
                throw new GenericValidationException("Invalid parameters.");

            response.Reviews = await _reviewRepository.GetByAuthorIdAsync(request.userId);
            return response;
        }
    }
}