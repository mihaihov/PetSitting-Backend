using FluentValidation;
using PetSitting.Application.Features.Stripe.Queries;

namespace PetSitting.Application.Features.Stripe.Validators
{
    public class QueryStripeTransactionsByUserValidator : AbstractValidator<QueryStripeTransactionsByUser>
    {
        public QueryStripeTransactionsByUserValidator()
        { 
            //email and user cannot be both null at the same time.
            RuleFor(st => st).Must(st => !(string.IsNullOrEmpty(st.email) && st.user == null));
        }
    }
}