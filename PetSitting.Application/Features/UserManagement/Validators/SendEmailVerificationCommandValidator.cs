using FluentValidation;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Commands;
using PetSitting.Application.Features.UserManagement.Entities;

namespace PetSitting.Application.Features.UserManagement.Validators
{
    public class SendEmailVerificationCommandValidator : AbstractValidator<UserManagementBaseCommand<BaseResponse>>
    {
        public SendEmailVerificationCommandValidator()
        {
            RuleFor(p => p.FirebaseToken)
                .NotEmpty().WithMessage("Firebase Token cannot be empty")
                .NotNull().WithMessage("Firebase Token cannot be null");
        }
    }
}