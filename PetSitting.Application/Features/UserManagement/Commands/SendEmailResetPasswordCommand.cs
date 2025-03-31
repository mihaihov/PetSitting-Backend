using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Application.Interfaces.Services;

namespace PetSitting.Application.Features.UserManagement.Commands
{
    public class SendEmailResetPasswordCommandHandler : BaseCommandHandler<UserManagementBaseCommand<BaseResponse>,BaseResponse>
    {
        private readonly IFirebaseService _firebaseService;
        public SendEmailResetPasswordCommandHandler(IFirebaseService firebaseService, IUserRepository userRepository)
        {
            _firebaseService = firebaseService;
        }
        protected override async Task<BaseResponse> HandleCommand(UserManagementBaseCommand<BaseResponse> request, BaseResponse response, CancellationToken cancellationToken)
        {
            try
            {
                if(string.IsNullOrEmpty(request.FirebaseToken))
                    throw new Exception("Invalid Token");

                var tokenVerificationResult = await _firebaseService.VerifyTokenAsync(request.FirebaseToken);
                if(tokenVerificationResult == null)
                    throw new Exception("Token verification failed!");
                
                await _firebaseService.SendPasswordResetEmailAsync(request.FirebaseToken);
                return response;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}