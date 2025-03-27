using MediatR;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Validators;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Application.Interfaces.Services;

namespace PetSitting.Application.Features.UserManagement.Commands
{
    public record SendEmailResetPasswordCommand(string email): IRequest<BaseResponse>;

    public class SendEmailResetPasswordCommandHandler : IRequestHandler<SendEmailResetPasswordCommand, BaseResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IFirebaseService _firebaseService;
        public SendEmailResetPasswordCommandHandler(IFirebaseService firebaseService, IUserRepository userRepository)
        {
            _firebaseService = firebaseService;
            _userRepository = userRepository;
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

                var sqlUser = await _userRepository.GetByEmailAsync(request.email);
                if(sqlUser == null)
                    throw new Exception("User not found in the database!");
                
                var firebaseToken = await _firebaseService.CreateCustomTokenAsync(sqlUser.Id);
                if(string.IsNullOrEmpty(firebaseToken)) throw new Exception("Could not create firebase token!");
                
                await _firebaseService.SendPasswordResetEmailAsync(firebaseToken);
                return response;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}