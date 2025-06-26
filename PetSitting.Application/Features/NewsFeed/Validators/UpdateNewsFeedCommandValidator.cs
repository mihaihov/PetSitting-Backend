namespace PetSitting.Application.Features.NewsFeed.Validators
{
    using FluentValidation;
    using PetSitting.Application.Features.NewsFeed.Commands;
    using PetSitting.Domain.Entities.PostManagement;

    public class UpdateNewsFeedCommandValidator : AbstractValidator<UpdateNewsFeedCommand>
    {
        public UpdateNewsFeedCommandValidator()
        {
            RuleFor(x => x.userId).NotNull().NotEmpty();
            RuleFor(x => x.postId).NotNull().NotEmpty();
        }
    }
}