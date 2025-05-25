using MediatR;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.Stripe.Validators;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Services;

namespace PetSitting.Application.Features.Stripe.Commands
{
    public record GenerateAccountLinkCommand(string accountId, string refreshUrl, string returnUrl)
        : IRequest<GenerateAccountLinkCommandResponse>;
    public record GenerateAccountLinkCommandResponse : BaseResponse
    {
        public string? Link {get;set;}
    }

    public class GenerateaccountLinkCommandHandler : BaseHandler<GenerateAccountLinkCommand,
        GenerateAccountLinkCommandResponse, GenerateAccountLinkCommandValidator>
    {
        private readonly IStripeServices _stripeServices;
        public GenerateaccountLinkCommandHandler(IStripeServices stripeServices)
        {
            _stripeServices = stripeServices;
        }
        protected override async Task<GenerateAccountLinkCommandResponse> HandleCommand(GenerateAccountLinkCommand request, GenerateAccountLinkCommandResponse response, CancellationToken cancellationToken)
        {
            response.Link = await _stripeServices.GenerateAccountLink(request.accountId,request.refreshUrl, request.returnUrl);
            return response;
        }
    }
}