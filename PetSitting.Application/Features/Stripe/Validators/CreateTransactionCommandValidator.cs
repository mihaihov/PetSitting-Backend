using FluentValidation;
using PetSitting.Application.Features.Stripe.Commands;

namespace PetSitting.Application.Features.Stripe.Validators
{
    public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionCommandValidator()
        {
            RuleFor(p => p.ammount).NotNull().Must(ammount => ammount>0.0m);
            RuleFor(p => p.currency).NotNull().NotEmpty();
            RuleFor(p => p.transactionId).NotNull().NotEmpty();
        }
    }
}