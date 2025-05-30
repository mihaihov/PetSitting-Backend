using System.Data.Common;
using MediatR;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.Stripe.Validators;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.Stripe;
using PetSitting.Domain.Entities.UserManagement;

namespace PetSitting.Application.Features.Stripe.Queries
{
    public record QueryStripeTransactionsByUser(string? email, ApplicationUser? user) : IRequest<QueryStripeTransactionsByUserResponse>;
    public record QueryStripeTransactionsByUserResponse : BaseResponse
    {
        public IReadOnlyList<StripeTransaction>? StripeTransactions {get;set;}
    }

    public class QueryStripeTransactionsByUserHandler : BaseHandler<QueryStripeTransactionsByUser,
                                                                      QueryStripeTransactionsByUserResponse,
                                                                      QueryStripeTransactionsByUserValidator>
    {
        private readonly IStripeTransactionRepository _stripeTransactionRepository;
        public QueryStripeTransactionsByUserHandler(IStripeTransactionRepository stripeTransactionRepository)
        {
            _stripeTransactionRepository = stripeTransactionRepository;
        }
        protected override async Task<QueryStripeTransactionsByUserResponse> HandleCommand(QueryStripeTransactionsByUser request, QueryStripeTransactionsByUserResponse response, CancellationToken cancellationToken)
        {
            response.StripeTransactions = string.IsNullOrEmpty(request.email) ? await _stripeTransactionRepository.GetAllByUser(request.user!)
                : await _stripeTransactionRepository.GetAllByUser(request.email!);
            return response;
        }
    }
}