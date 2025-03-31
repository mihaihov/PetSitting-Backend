using FluentValidation;
using MediatR;
using PetSitting.Application.Features.Common;

namespace PetSitting.Application.Features.UserManagement.Entities
{
    //used with a validator.
    public abstract class BaseCommandHandler<TRequest, TResponse, TValidator> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : BaseResponse, new()
        where TValidator: AbstractValidator<TRequest>, new()
    {

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var response = new TResponse();
            var validator = new TValidator();
            var validationResult = await validator.ValidateAsync(request);

            if(!validationResult.IsValid)
            {
                response.Success = false;
                response.ValidationErrors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return response;
            }

            return await HandleCommand(request,response, cancellationToken);
        }

        protected abstract Task<TResponse> HandleCommand(TRequest request, TResponse response, CancellationToken cancellationToken);
    }

    //used without a validator.
    public abstract class BaseCommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : BaseResponse, new()
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var response = new TResponse();

            return await HandleCommand(request,response, cancellationToken);
        }

        protected abstract Task<TResponse> HandleCommand(TRequest request, TResponse response, CancellationToken cancellationToken);
    }
}