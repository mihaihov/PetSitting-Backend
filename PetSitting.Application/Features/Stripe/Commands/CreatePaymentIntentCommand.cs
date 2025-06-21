using Firebase.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.Stripe.Validators;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Application.Interfaces.Services;
using PetSitting.Domain.Entities.Stripe;
using PetSitting.Domain.Entities.UserManagement;
using Stripe;

namespace PetSitting.Application.Features.Stripe.Commands
{
    public record CreatePaymentIntentCommand(long amount, string currency, string destinationAccount,
        string? destinationEmail,string? jobPostId) : IRequest<CreatePaymentIntentCommandResponse>;
    public record CreatePaymentIntentCommandResponse : BaseResponse
    {
        public string? ClientSecret {get;set;}
    }

    public class CreatePaymentIntentCommandHandler : BaseHandler<CreatePaymentIntentCommand,CreatePaymentIntentCommandResponse,CreatePaymentIntentCommandValidator>
    {
        private readonly IStripeServices _stripeServices;
        private readonly IStripeTransactionRepository _stripeTransactionRepository;
        private readonly IUserRepository _userRepoistory;
        public CreatePaymentIntentCommandHandler(IStripeServices stripeServices, IStripeTransactionRepository stripeTransactionRepository, IUserRepository userRepository)
        {
            _stripeServices = stripeServices;
            _stripeTransactionRepository = stripeTransactionRepository;
            _userRepoistory = userRepository;
        }
        protected override async Task<CreatePaymentIntentCommandResponse> HandleCommand(CreatePaymentIntentCommand request, CreatePaymentIntentCommandResponse response,
            CancellationToken cancellationToken)
        {
            //BASED ON THIS PAYMENT INTENT, A TRANSACTION IS CREATED AND STORED IN THE DB WHICH ONLY HAS THE LINK TO THE JOBPOST. 
            //THE REST OF THE PROPERTIES OF THE TRANSACTION WILL BE COMPLETED IN THE StripeWebhookController, when a webhook of type
            //PaymentIntentCreated arrives.
            //Internal StripeTransaction ID is sent to Stripe as metadata. In this way I can retrieve internal StripeTransaction
                //from my DB and update it with other information from webhook payload.
            var transaction = new StripeTransaction
            {
                JobPostId = request.jobPostId
            };
            string? destinationStripeAccountId;
            ApplicationUser? destinationStripeAccount;

            if(!string.IsNullOrEmpty(request.destinationEmail))
                destinationStripeAccount = await _userRepoistory.QueryByEmailAsync(request.destinationEmail).Include(u => u.StripeAccount).FirstOrDefaultAsync();
            else
                destinationStripeAccount = await _userRepoistory.QueryByIdAsync(request.destinationAccount).Include(u => u.StripeAccount).FirstOrDefaultAsync();

            if(destinationStripeAccount is null)    throw new GenericValidationException("Destination account is invalid.");
            if(destinationStripeAccount.StripeAccountId is null)    throw new GenericValidationException("Destination account is invalid.");
            destinationStripeAccountId = destinationStripeAccount.Id;


            var paymentIntent = await _stripeServices.CreatePaymentIntent(request.amount,request.currency,destinationStripeAccountId,
                request.destinationEmail);
            if(paymentIntent is null)   throw new CannotCreatePaymentIntentException();
            transaction.PaymentIntentId = paymentIntent.Id;
            await _stripeTransactionRepository.AddAsync(transaction);

            response.ClientSecret = paymentIntent.ClientSecret;
            return response;
        }
    }
}