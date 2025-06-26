using FluentValidation;
using PetSitting.Application.Features.PostManagement.Commands;

namespace PetSitting.Application.Features.PostManagement.Validators
{
    public class CreateJobPostCommandValidator : AbstractValidator<CreateJobPostCommand>
    {
        public CreateJobPostCommandValidator()
        {
            RuleFor(p => p.JobPost.AuthorId)
                .NotEmpty().WithMessage("Author Id cannot be empty!")
                .NotNull().WithMessage("Author Id cannot be null!");
            RuleFor(p => p.JobPost.Title)
                .NotEmpty().WithMessage("Title cannot be empty!")
                .NotNull().WithMessage("Title cannot be null!")
                .MaximumLength(30).WithMessage("Title is too long!");
            RuleFor(p => p.JobPost.Location)
                .NotNull().WithMessage("Location cannot be null!")
                .NotEmpty().WithMessage("Location cannot be empty!");
            RuleFor(p => p.JobPost.StartDate).NotNull()
                .WithMessage("Start date cannot be null");
            RuleFor(p => p.JobPost.EndDate).NotNull()
                .WithMessage("End date cannot be null");
            RuleFor(p => p.JobPost.Payment).NotEqual(0).WithMessage("Payment cannot be null, If you do not want to specify it, make it null!");

        }
    }
}