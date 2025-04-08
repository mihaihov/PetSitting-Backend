using FluentValidation;
using PetSitting.Application.Features.PostManagement.Commands;

namespace PetSitting.Application.Features.PostManagement.Validators
{
    public class UpdateJobOfferStatusCommandValidator : AbstractValidator<UpdateJobOfferStatusCommand>
    {
        public UpdateJobOfferStatusCommandValidator()
        {
            RuleFor(p => p.id).NotNull().NotEmpty().WithMessage("Invalid Job application id!");
            RuleFor(p => p.status).NotNull().NotEmpty().WithMessage("Invalid Job application status!");
        }
    }
}