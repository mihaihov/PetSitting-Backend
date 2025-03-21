using FluentValidation;

namespace PetSitting.Application.Features.UserManagement.Validators
{
    public class LoginWithCredentialsCommandValidator : AbstractValidator<LoginWithCredentialsCommand>
    {
        public LoginWithCredentialsCommandValidator()
        {
            RuleFor(p => p.email)
                .NotEmpty().WithMessage("Email cannot be empty")
                .NotNull().WithMessage("Email cannot be null")
                .EmailAddress().WithMessage("Must be a valid email adress");

            RuleFor(p => p.password)
                .NotEmpty().WithMessage("Password cannot be empty")
                .NotNull().WithMessage("Password cannot be null")
                .Matches("[A-Za-z]").WithMessage("Password must contain at least one letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");
        }
    }
}