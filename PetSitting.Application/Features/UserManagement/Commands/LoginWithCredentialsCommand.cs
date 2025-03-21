using MediatR;
using PetSitting.Application.Common;
using PetSitting.Application.Features.UserManagement.Validators;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Application.Interfaces.Services;

namespace PetSitting.Application.Features.UserManagement
{
    public record LoginWithCredentialsCommand(string email, string password) : IRequest<LoginWithCredentialsCommandResponse>;
    public record LoginWithCredentialsCommandResponse : BaseResponse;

    public class LoginWithCredentialsCommandHandler : IRequestHandler<LoginWithCredentialsCommand, LoginWithCredentialsCommandResponse>
    {
        private readonly IFirebaseServices _firebaseService;
        private readonly IUserRepository _userRepository;
        public LoginWithCredentialsCommandHandler(IFirebaseServices firebaseService, IUserRepository userRepository)
        {
            _firebaseService = firebaseService;
            _userRepository = userRepository;
        }

        public async Task<LoginWithCredentialsCommandResponse> Handle(LoginWithCredentialsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                LoginWithCredentialsCommandResponse response = new LoginWithCredentialsCommandResponse();
                var validator = new LoginWithCredentialsCommandValidator();
                var validationResult = await validator.ValidateAsync(request);

                if (validationResult.Errors.Any())
                {
                    response.Success = false;
                    response.ValidationErrors = new List<string>();
                    foreach (var error in validationResult.Errors)
                        response.ValidationErrors.Add(error.ErrorMessage);
                    return response;
                }

                // var firebaseUser = await _firebaseService.GetUserByEmailAsync(request.email);
                // if (firebaseUser == null)
                //     throw new Exception("User not found");
                
                // var sqlUser = await _userRepository.GetByIdAsync(firebaseUser.Uid);
                // if(sqlUser == null)
                //     throw new Exception("User not found!");
                var signIn = await _firebaseService.SignInWithEmailAndPasswordAsync(request.email,request.password);
                if(signIn == null)
                    throw new Exception("LogIn failed");


                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}