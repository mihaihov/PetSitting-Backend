using FluentValidation;
using PetSitting.Application.Features.Stripe.Commands;

namespace PetSitting.Application.Features.Stripe.Validators
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(p => p.email).NotEmpty().NotNull().EmailAddress();
        }
    }
}