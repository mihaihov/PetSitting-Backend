using MediatR;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Validators;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Application.Interfaces.Services;

namespace PetSitting.Application.Features.UserManagement.Commands
{
    public record SendEmailResetPasswordCommand(string firebaseToken): IRequest<BaseResponse>;

    public class SendEmailResetPasswordCommandHandler : IRequestHandler<SendEmailResetPasswordCommand, BaseResponse>
    {
        private readonly IFirebaseService _firebaseService;
        public SendEmailResetPasswordCommandHandler(IFirebaseService firebaseService, IUserRepository userRepository)
        {
            _firebaseService = firebaseService;
        }
        public async Task<BaseResponse> Handle(SendEmailResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                BaseResponse response = new BaseResponse();
                var validator = new SendEmailResetPasswordCommandValidator();
                var validationResult = await validator.ValidateAsync(request);

                if (validationResult.Errors.Count > 0)
                {
                    response.Success = false;
                    response.ValidationErrors = new List<string>();
                    foreach (var error in validationResult.Errors)
                        response.ValidationErrors.Add(error.ErrorMessage);
                    return response;
                }
                var tokenVerificationResult = await _firebaseService.VerifyTokenAsync(request.firebaseToken);
                if(tokenVerificationResult == null)
                    throw new Exception("Token verification failed!");
                
                await _firebaseService.SendPasswordResetEmailAsync(request.firebaseToken);
                return response;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}