using FluentValidation;
using PetSitting.Application.Features.Stripe.Commands;

namespace PetSitting.Application.Features.Stripe.Validators
{
    public class CreatePaymentIntentCommandValidator : AbstractValidator<CreatePaymentIntentCommand>
    {
        public CreatePaymentIntentCommandValidator()
        {
            RuleFor(p => p.amount).NotEmpty().NotNull();
            RuleFor(p => p.currency).NotEmpty().NotNull();
            RuleFor(p => p.paymentMethodId).NotEmpty().NotNull();
            RuleFor(p => p.destinationAccount).NotEmpty().NotNull();
            RuleFor(p => p.destinationEmail).EmailAddress();
        }
    }
}