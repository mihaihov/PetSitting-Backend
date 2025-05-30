using FluentValidation;
using PetSitting.Application.Features.Stripe.Queries;

namespace PetSitting.Application.Features.Stripe.Validators
{
    public class QueryStripeTransactionsByJobPostValidator : AbstractValidator<QueryStripeTransactionsByJobPost>
    {
        public QueryStripeTransactionsByJobPostValidator()
        {
            RuleFor(jp => jp.jobPostId).NotNull().NotEmpty();
        }
    }
}