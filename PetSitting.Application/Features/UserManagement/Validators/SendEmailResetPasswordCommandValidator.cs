using FluentValidation;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Commands;

namespace PetSitting.Application.Features.UserManagement.Validators
{
    public class SendEmailResetPasswordCommandValidator : AbstractValidator<SendEmailResetPasswordCommand>
    {
        public SendEmailResetPasswordCommandValidator()
        {
            RuleFor(p => p.firebaseToken)
                .NotEmpty().WithMessage("Token cannot be empty")
                .NotNull().WithMessage("Token cannot be null");
        }
    }
}