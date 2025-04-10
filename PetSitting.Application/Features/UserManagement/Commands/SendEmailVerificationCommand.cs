using PetSitting.Application.Exceptions;
using PetSitting.Application.Exceptions.Firebase;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Services;

namespace PetSitting.Application.Features.UserManagement.Commands
{
    public class SendEmailVerificationCommandHandler : BaseHandler<UserManagementBaseCommand<BaseResponse>,BaseResponse>
    {
        private readonly IFirebaseService _firebaseService;
        public SendEmailVerificationCommandHandler(IFirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }
        protected override async Task<BaseResponse> HandleCommand(UserManagementBaseCommand<BaseResponse> request, BaseResponse response, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.FirebaseToken))
                throw new GenericValidationException("Invalid token!");

            var tokenVerificationResult = await _firebaseService.VerifyTokenAsync(request.FirebaseToken);
            if (tokenVerificationResult == null)
                throw new FirebaseTokenValidationException();

            await _firebaseService.SendEmailVerificationAsync(request.FirebaseToken);

            return response;
        }
    }
}