using MediatR;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.Stripe.Validators;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.Stripe;

namespace PetSitting.Application.Features.Stripe.Commands
{
    public record CreateTransactionCommand(string paymentIntentId, string? status, decimal? ammount,
        string? currency, DateTime? createdAt, string? paidTo, string? paidBy) : IRequest<BaseResponse>;

    public class CreateTransactionCommandHandler : BaseHandler<CreateTransactionCommand, BaseResponse, CreateTransactionCommandValidator>
    {
        private readonly IStripeTransactionRepository _stripeTransactionRepository;
        public CreateTransactionCommandHandler(IStripeTransactionRepository stripeTransactionRepository)
        {
            _stripeTransactionRepository = stripeTransactionRepository;
        }
        protected override async Task<BaseResponse> HandleCommand(CreateTransactionCommand request, BaseResponse response, CancellationToken cancellationToken)
        {
            var transaction = await _stripeTransactionRepository.GetByPaymentIntentId(request.paymentIntentId);
            if(transaction == null)
                throw new GenericValidationException("Stripe transaction not found in the internal Database.");

            transaction.PaymentIntentId = request.paymentIntentId;
            transaction.Status = request.status;
            transaction.Amount = request.ammount / 100m; //Convert from cents to dollars
            transaction.Currency = request.currency;
            transaction.CreatedAt = request.createdAt;
            transaction.PaidToId = request.paidTo;
            transaction.PaidById = request.paidBy;

            //It should already exist. See CreatePaymentIntentcommand, HandleCommand method.
            await _stripeTransactionRepository.UpdateAsync(transaction);

            return response;
        }
    }
}