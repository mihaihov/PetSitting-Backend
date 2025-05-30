using MediatR;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.Stripe.Validators;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.Stripe;

namespace PetSitting.Application.Features.Stripe.Queries
{
    public record QueryStripeTransactionsByJobPost(string jobPostId) : IRequest<QueryStripeTransactionsByJobPostResponse>;
    public record QueryStripeTransactionsByJobPostResponse : BaseResponse
    {
        public IReadOnlyList<StripeTransaction>? StripeTransactions {get;set;}
    }
    public class QueryStripeTransactionsByJobPostHandler : BaseHandler<QueryStripeTransactionsByJobPost,
                                                                       QueryStripeTransactionsByJobPostResponse,
                                                                       QueryStripeTransactionsByJobPostValidator>
    {
        private readonly IStripeTransactionRepository _stripeTransactionRepository;
        public QueryStripeTransactionsByJobPostHandler(IStripeTransactionRepository stripeTransactionRepository)
        {
            _stripeTransactionRepository = stripeTransactionRepository;
        }
        protected override async Task<QueryStripeTransactionsByJobPostResponse> HandleCommand(QueryStripeTransactionsByJobPost request, QueryStripeTransactionsByJobPostResponse response, CancellationToken cancellationToken)
        {
            response.StripeTransactions = await _stripeTransactionRepository.GetAllByJobPost(request.jobPostId);
            return response;
        }
    }
}