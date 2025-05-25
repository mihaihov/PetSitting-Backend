using FluentValidation;
using PetSitting.Application.Features.Stripe.Commands;

namespace PetSitting.Application.Features.Stripe.Validators
{
    public class GenerateAccountLinkCommandValidator : AbstractValidator<GenerateAccountLinkCommand>
    {
        public GenerateAccountLinkCommandValidator()
        {
            RuleFor(p => p.accountId).NotNull().NotEmpty();
            RuleFor(p => p.refreshUrl).NotNull().NotEmpty();
            RuleFor(p => p.returnUrl).NotNull().NotEmpty();
        }
    }
}