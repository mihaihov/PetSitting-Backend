using MediatR;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.Stripe.Validators;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Services;

namespace PetSitting.Application.Features.Stripe.Commands
{
    public record CreateAccountCommand(string email) : IRequest<CreateAccountCommandResponse>;

    public record CreateAccountCommandResponse : BaseResponse
    {
        public string? AccountId {get;set;}
    }

    public class CreateStripeAccountCommandHandler : BaseHandler<CreateAccountCommand
        ,CreateAccountCommandResponse, CreateAccountCommandValidator>
    {
        private readonly IStripeServices _stripeServices;
        public CreateStripeAccountCommandHandler(IStripeServices stripeServices)
        {
            _stripeServices = stripeServices;
        }
        protected override async Task<CreateAccountCommandResponse> HandleCommand(CreateAccountCommand request, CreateAccountCommandResponse response, CancellationToken cancellationToken)
        {
            //check to see if an account for this email already exists.
                //it was not confirmed. if so, send another confirmation email
            
            response.AccountId = await _stripeServices.CreateAccount(request.email);
            return response;
        }
    }


}