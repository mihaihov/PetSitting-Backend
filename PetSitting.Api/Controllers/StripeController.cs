using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Stripe.Commands;
using PetSitting.Application.Features.Stripe.Queries;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.Stripe;

namespace PetSitting.Api.Controllers
{
    [Route("api/[controller]/")]
    public class StripeController : BaseController
    {
        private readonly IStripeTransactionRepository _stripeTransactionRepository;
        public StripeController(IStripeTransactionRepository stripeTransactionRepository, IMediator mediator) 
            : base(mediator) 
        {
            _stripeTransactionRepository = stripeTransactionRepository;
        }

        [HttpGet("getbyid")]
        private async Task<ActionResult<StripeTransaction?>> GetById(string transactionId)
        {
            if(string.IsNullOrEmpty(transactionId)) throw new GenericValidationException("Transaction id cannot be empty.");
                
            var transaction = await _stripeTransactionRepository.GetByIdAsync(transactionId);
            return Ok(transaction);
        }

        [HttpPost("createstripeaccount")]
        public Task<ActionResult<CreateAccountCommandResponse>> CreateStripeAccount(CreateAccountCommand command) =>
            HandleRequest<CreateAccountCommand,CreateAccountCommandResponse>(command);

        [HttpPost("createpaymentintent")]
        public Task<ActionResult<CreatePaymentIntentCommandResponse>> CreatePaymentIntent(CreatePaymentIntentCommand command) =>
            HandleRequest<CreatePaymentIntentCommand,CreatePaymentIntentCommandResponse>(command);

        [HttpGet("getallbyuser")]
        public Task<ActionResult<QueryStripeTransactionsByUserResponse>> GetAllByUser(QueryStripeTransactionsByUser query) =>
            HandleRequest<QueryStripeTransactionsByUser,QueryStripeTransactionsByUserResponse>(query);

        [HttpGet("getallbyjobpost")]
        public Task<ActionResult<QueryStripeTransactionsByJobPostResponse>> GetAllByJobPost(QueryStripeTransactionsByJobPost query) =>
            HandleRequest<QueryStripeTransactionsByJobPost,QueryStripeTransactionsByJobPostResponse>(query);
    }
}