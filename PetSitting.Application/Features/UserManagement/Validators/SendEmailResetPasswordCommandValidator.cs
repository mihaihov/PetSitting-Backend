using FluentValidation;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Commands;
using PetSitting.Application.Features.UserManagement.Entities;

namespace PetSitting.Application.Features.UserManagement.Validators
{
    public class SendEmailResetPasswordCommandValidator : AbstractValidator<UserManagementBaseCommand<BaseResponse>>
    {
        public SendEmailResetPasswordCommandValidator()
        {
            RuleFor(p => p.FirebaseToken)
                .NotEmpty().WithMessage("Token cannot be empty")
                .NotNull().WithMessage("Token cannot be null");
        }
    }
}