using FluentValidation;
using PetSitting.Application.Features.UserManagement.Commands;

namespace PetSitting.Application.Features.UserManagement.Validators
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(p => p.firebaseToken)
                .NotNull().WithMessage("Token cannot be null!")
                .NotEmpty().WithMessage("Token cannot be empty!");

            RuleFor(p => p.newPassword)
                .NotEmpty().WithMessage("Password cannot be empty")
                .NotNull().WithMessage("Password cannot be null")
                .Matches("[A-Za-z]").WithMessage("Password must contain at least one letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");
        }
    }
}