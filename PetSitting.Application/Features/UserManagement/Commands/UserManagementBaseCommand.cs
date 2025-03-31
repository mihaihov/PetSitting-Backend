using FluentValidation;
using MediatR;
using PetSitting.Application.Features.Common;

namespace PetSitting.Application.Features.UserManagement.Commands
{
    public abstract class UserManagementBaseCommand<TRequest, TResponse, TValidator> : IRequestHandler<TRequest, TResponse>
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
}