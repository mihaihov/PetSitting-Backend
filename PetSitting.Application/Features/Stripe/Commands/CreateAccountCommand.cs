using MediatR;
using Microsoft.EntityFrameworkCore;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.Stripe.Validators;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Application.Interfaces.Services;
using PetSitting.Domain.Entities.Stripe;

namespace PetSitting.Application.Features.Stripe.Commands
{
    public record CreateAccountCommand(string email, string refreshUrl, string returnUrl) : IRequest<CreateAccountCommandResponse>;

    public record CreateAccountCommandResponse : BaseResponse
    {
        public string? AccountLinkUrl {get;set;}
    }

    public class CreateStripeAccountCommandHandler : BaseHandler<CreateAccountCommand
        ,CreateAccountCommandResponse, CreateAccountCommandValidator>
    {
        private readonly IStripeServices _stripeServices;
        private readonly IUserRepository _userRepository;
        public CreateStripeAccountCommandHandler(IStripeServices stripeServices, IUserRepository userRepository)
        {
            _stripeServices = stripeServices;
            _userRepository = userRepository;
        }
        protected override async Task<CreateAccountCommandResponse> HandleCommand(CreateAccountCommand request, CreateAccountCommandResponse response, CancellationToken cancellationToken)
        {
            var stripeAccountId = string.Empty;

            try
            {
                var user = await _userRepository.QueryByEmailAsync(request.email).Include(u => u.StripeAccount).FirstOrDefaultAsync();
                if (user == null)
                    throw new InternalUserNotFoundException();

                if (string.IsNullOrEmpty(user?.StripeAccount?.Id))
                {
                    stripeAccountId = await _stripeServices.CreateAccount(request.email);
                    StripeAccount stripeAccount = new StripeAccount
                    {
                        Id = stripeAccountId,
                        ApplicationUserId = user!.Id,
                        ApplicationUser = user!,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    user.StripeAccount = stripeAccount;
                    await _userRepository.Update(user);
                }

                response.AccountLinkUrl = await _stripeServices.GenerateAccountLink(stripeAccountId, request.refreshUrl, request.returnUrl);

                return response;
            }
            catch(Exception)
            {
                await _userRepository.RollbackTransactionAsync();
                throw;
            }
        }
    }


}