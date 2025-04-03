using FluentValidation;
using PetSitting.Application.Features.PostManagement.Commands;

namespace PetSitting.Application.Features.PostManagement.Validators
{
    public class CreateJobPostCommandValidator : AbstractValidator<CreateJobPostCommand>
    {
        public CreateJobPostCommandValidator()
        {
            RuleFor(p => p.authorId)
                .NotEmpty().WithMessage("Author Id cannot be empty!")
                .NotNull().WithMessage("Author Id cannot be null!");
            RuleFor(p => p.title)
                .NotEmpty().WithMessage("Title cannot be empty!")
                .NotNull().WithMessage("Title cannot be null!")
                .MaximumLength(30).WithMessage("Title is too long!");
            RuleFor(p => p.location)
                .NotNull().WithMessage("Location cannot be null!")
                .NotEmpty().WithMessage("Location cannot be empty!");
            RuleFor(p => p.startDate).NotNull()
                .WithMessage("Start date cannot be null");
            RuleFor(p => p.endDate).NotNull()
                .WithMessage("End date cannot be null");
            RuleFor(p => p.payment).NotEqual(0).WithMessage("Payment cannot be null, If you do not want to specify it, make it null!");

        }
    }
}