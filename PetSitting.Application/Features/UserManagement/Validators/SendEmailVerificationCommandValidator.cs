using FluentValidation;
using PetSitting.Application.Features.UserManagement.Commands;

namespace PetSitting.Application.Features.UserManagement.Validators
{
    public class SendEmailVerificationCommandValidator : AbstractValidator<SendEmailVerificationCommand>
    {
        public SendEmailVerificationCommandValidator()
        {
            RuleFor(p => p.firebaseToken)
                .NotEmpty().WithMessage("Firebase Token cannot be empty")
                .NotNull().WithMessage("Firebase Token cannot be null");
        }
    }
}