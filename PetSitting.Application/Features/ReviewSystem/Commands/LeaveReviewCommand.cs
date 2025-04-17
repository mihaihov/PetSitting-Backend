using MediatR;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.ReviewSystem.Validators;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.ReviewSystem;

namespace PetSitting.Application.Features.ReviewSystem.Commands
{
    public record LeaveReviewCommand(string title,string content,int rating,
        string authorId, string postId) : IRequest<BaseResponse>;
    public class LeaveReviewCommandHandler : BaseHandler<LeaveReviewCommand, BaseResponse, LeaveReviewCommandValidator>
    {
        private readonly IReviewRepository _reviewRepository;
        public LeaveReviewCommandHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        protected override async Task<BaseResponse> HandleCommand(LeaveReviewCommand request, BaseResponse response, CancellationToken cancellationToken)
        {
            var reviewByUserCount = await _reviewRepository.GetReviewsByAuthorToPost(request.authorId,request.postId);
            if(reviewByUserCount?.Count > 1) 
                throw new CannotReviewException("You already reviewd this post!");

            await _reviewRepository.AddAsync(Review.CreateReview(
                request.title,request.content,null,null,null,request.rating,request.authorId,request.postId
            ));
            return response; 
        }
    }
}