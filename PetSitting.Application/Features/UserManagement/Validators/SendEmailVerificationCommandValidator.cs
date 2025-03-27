using FluentValidation;
using PetSitting.Application.Features.UserManagement.Commands;

namespace PetSitting.Application.Features.UserManagement.Validators
{
    public class SendEmailVerificationCommandValidator : AbstractValidator<SendEmailVerificationCommand>
    {
        public SendEmailVerificationCommandValidator()
        {
            RuleFor(p => p.email)
                .NotEmpty().WithMessage("Email cannot be empty")
                .NotNull().WithMessage("Email cannot be null")
                .EmailAddress().WithMessage("Must be a valid email adress");
        }
    }
}