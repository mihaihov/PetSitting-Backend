using MediatR;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Validators;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Application.Interfaces.Services;

namespace PetSitting.Application.Features.UserManagement.Commands
{
    public record SendEmailVerificationCommand(string email) : IRequest<BaseResponse>;

    public class SendEmailVerificationCommandHandler : IRequestHandler<SendEmailVerificationCommand, BaseResponse>
    {
        private readonly IFirebaseService _firebaseService;
        private readonly IUserRepository _userRepository;
        public SendEmailVerificationCommandHandler(IFirebaseService firebaseService, IUserRepository userRepository)
        {
            _firebaseService = firebaseService;
            _userRepository = userRepository;
        }
        public async Task<BaseResponse> Handle(SendEmailVerificationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                BaseResponse response = new BaseResponse();
                var validator = new SendEmailVerificationCommandValidator();
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
                if (sqlUser == null) throw new Exception("User not found in the database!");

                var firebaseToken = await _firebaseService.CreateCustomTokenAsync(sqlUser.Id); 
                if(firebaseToken == null)
                    throw new Exception("Could not create custom firebase token!");

                await _firebaseService.SendEmailVerificationAsync(firebaseToken);               
                
                return response;
            }
            catch(Exception)
            {
                throw;
            }

        }
    }
}