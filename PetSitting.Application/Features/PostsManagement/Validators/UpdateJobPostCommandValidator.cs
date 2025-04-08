using FluentValidation;
using PetSitting.Application.Features.PostManagement.Commands;

namespace PetSitting.Application.Features.PostManagement.Validators
{
    public class UpdateJobPostCommandValidator : AbstractValidator<UpdateJobPostCommand>
    {
        public UpdateJobPostCommandValidator()
        {
            RuleFor(p => p.id)
                .NotNull().WithMessage("Id cannot be null!")
                .NotEmpty().WithMessage("Id cannot be empty!");
            RuleFor(p => p.payment).NotEqual(0).WithMessage("Payment cannot be null, If you do not want to specify it, make it null!");
        }
    }
}