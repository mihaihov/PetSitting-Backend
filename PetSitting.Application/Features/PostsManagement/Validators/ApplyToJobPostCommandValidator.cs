using FluentValidation;
using PetSitting.Application.Features.PostManagement.Commands;

namespace PetSitting.Application.Features.PostManagement.Validators
{
    public class ApplyToJobPostCommandValidator : AbstractValidator<ApplyToJobPostCommand>
    {
        public ApplyToJobPostCommandValidator()
        {
            RuleFor(p => p.jobPostId)
                .NotNull().WithMessage("Job post Id cannot be null!")
                .NotEmpty().WithMessage("Job post Id cannot be empty!");
            
            RuleFor(p => p.applicantId)
                .NotNull().WithMessage("Applicant Id cannot be null!")
                .NotEmpty().WithMessage("Applicant Id cannot be empty!");
        }
    }
}