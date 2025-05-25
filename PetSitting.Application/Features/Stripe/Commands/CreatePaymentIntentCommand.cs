using MediatR;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.Stripe.Validators;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Services;
using Stripe;

namespace PetSitting.Application.Features.Stripe.Commands
{
    public record CreatePaymentIntentCommand(long amount, string currency, string paymentMethodId, string destinationAccount,
        string? destinationEmail) : IRequest<CreatePaymentIntentCommandResponse>;
    public record CreatePaymentIntentCommandResponse : BaseResponse
    {
        public PaymentIntent? PaymentIntent {get;set;}
    }

    public class CreatePaymentIntentCommandHandler : BaseHandler<CreatePaymentIntentCommand,CreatePaymentIntentCommandResponse,CreatePaymentIntentCommandValidator>
    {
        private readonly IStripeServices _stripeServices;
        public CreatePaymentIntentCommandHandler(IStripeServices stripeServices)
        {
            _stripeServices = stripeServices;
        }
        protected override async Task<CreatePaymentIntentCommandResponse> HandleCommand(CreatePaymentIntentCommand request, CreatePaymentIntentCommandResponse response,
            CancellationToken cancellationToken)
        {
            var paymentIntent = await _stripeServices.CreatePaymentIntent(request.amount,request.currency,request.paymentMethodId,request.destinationAccount,request.destinationEmail);
            if(paymentIntent is null)   throw new CannotCreatePaymentIntentException();
            response.PaymentIntent = paymentIntent;
            return response;
        }
    }
}