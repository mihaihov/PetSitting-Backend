using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetSitting.Application.Features.Stripe.Commands;
using PetSitting.Application.Features.Stripe.Queries;

namespace PetSitting.Api.Controllers
{
    [Route("/api/[controller]/")]
    public class StripeController : BaseController
    {
        public StripeController(IMediator mediator) : base(mediator) {}

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