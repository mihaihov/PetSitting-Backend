using MediatR;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;

namespace PetSitting.Application.Features.ReviewSystem.Commands
{
    public record UpdateReviewCommand(string reviewId,string userId, string postId, string? title,
        string? content, int? rating) : IRequest<BaseResponse>;
    public class UpdateReviewCommandHandler : BaseHandler<UpdateReviewCommand, BaseResponse>
    {
        private readonly IReviewRepository _reviewRepository;
        public UpdateReviewCommandHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        protected override async Task<BaseResponse> HandleCommand(UpdateReviewCommand request, BaseResponse response, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.userId) || string.IsNullOrEmpty(request.postId)
                ||string.IsNullOrEmpty(request.reviewId))
                throw new GenericValidationException("Invalid parameters.");
            
            var review = await _reviewRepository.GetByIdAsync(request.reviewId);
            if(review == null)
                throw new ReviewNotFoundException();

            if(review.UpdatedCount == 2) 
                throw new CannotUpdateException();
            
            if(!string.IsNullOrEmpty(request.title)) review.Title = request.title;
            if(!string.IsNullOrEmpty(request.content)) review.Title = request.content;
            if(request.rating != null) review.Rating = (int)request.rating;
            review.UpdatedAt = DateTime.Now;
            review.UpdatedCount ++;

            await _reviewRepository.UpdateAsync(review);
            return response;
        }
    }
}