using MediatR;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;

namespace PetSitting.Application.Features.ReviewSystem.Commands
{
    public record DeleteReviewCommand(string reviewId) : IRequest<BaseResponse>;

    public class DeleteReviewCommandHandler : BaseHandler<DeleteReviewCommand, BaseResponse>
    {
        private readonly IReviewRepository _reviewRepository;
        public DeleteReviewCommandHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        protected override async Task<BaseResponse> HandleCommand(DeleteReviewCommand request, BaseResponse response, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.reviewId))
                throw new GenericValidationException("Invalid parameters.");

            var review = await _reviewRepository.GetByIdAsync(request.reviewId);
            if(review == null)
                throw new ReviewNotFoundException();

            await _reviewRepository.Delete(review);

            return response;
        }
    }
}