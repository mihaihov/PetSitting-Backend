using FluentValidation;
using PetSitting.Application.Features.ReviewSystem.Commands;

namespace PetSitting.Application.Features.ReviewSystem.Validators
{
    public class LeaveReviewCommandValidator : AbstractValidator<LeaveReviewCommand>
    {
        public LeaveReviewCommandValidator()
        {
            RuleFor(p => p.title).NotEmpty().NotNull();
            RuleFor(p => p.content).NotEmpty().NotNull();
            RuleFor(p => p.rating).NotNull().InclusiveBetween(1,5);
            RuleFor(p => p.authorId).NotEmpty().NotNull();
            RuleFor(p => p.postId).NotEmpty().NotNull();
        }
    }
}