using MediatR;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Validators;
using PetSitting.Application.Interfaces.Services;

namespace PetSitting.Application.Features.UserManagement.Commands
{
    public record SendEmailVerificationCommand(string email) : IRequest<BaseResponse>;

    public class SendEmailVerificationCommandHandler : IRequestHandler<SendEmailVerificationCommand, BaseResponse>
    {
        private readonly IFirebaseService _firebaseService;
        public SendEmailVerificationCommandHandler(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }
        public async Task<BaseResponse> Handle(SendEmailVerificationCommand request, CancellationToken cancellationToken)
        {
            BaseResponse response = new BaseResponse();
            var validator = new SendEmailVerificationCommandValidator();
            var validationResult = await validator.ValidateAsync(request);

            if(validationResult.Errors.Count > 0)
            {
                response.Success = false;
                response.ValidationErrors = new List<string>();
                foreach (var error in validationResult.Errors)
                    response.ValidationErrors.Add(error.ErrorMessage);
                return response;
            }

            return response;            

        }
    }
}