using MediatR;
using Microsoft.Extensions.Options;
using PetSitting.Application.Common;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Application.Interfaces.Services;
using PetSitting.Domain.Entities.UserManagement;
using PetSitting.Domain.Entities.Utils;
using PetSitting.Domain.Features;

namespace PetSitting.Application.Features.UserManagement.Commands
{
    public record LoginWithFacebookCommand(string firebaseToken) : IRequest<LoginWithFacebookCommandResponse>;

    public record LoginWithFacebookCommandResponse : BaseResponse
    {
        public string? JWToken {get;set;} = null;
    }
    public class LoginWithFacebookCommandHandler : IRequestHandler<LoginWithFacebookCommand, LoginWithFacebookCommandResponse>
    {
        private readonly IFirebaseServices _firebaseService;
        private readonly IUserRepository _userRepository;
        private readonly IOptions<JwtSettings> _jwtSettings;
        public LoginWithFacebookCommandHandler(IFirebaseServices firebaseService, IUserRepository userRepository,
            IOptions<JwtSettings> jwtSettings)
        {
            _firebaseService = firebaseService;
            _userRepository = userRepository;
            _jwtSettings = jwtSettings;
        }

        public async Task<LoginWithFacebookCommandResponse> Handle(LoginWithFacebookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                LoginWithFacebookCommandResponse response = new LoginWithFacebookCommandResponse();

                //verifies firebase token agains the firebase database
                var firebaseTokenAuthenticity = await _firebaseService.VerifyTokenAsync(request.firebaseToken);
                if (firebaseTokenAuthenticity == null)
                    throw new Exception("Firebase user does not exist");

                //verifies if user exists in sql db; if it does not, creat it.
                var sqlUser = await _userRepository.GetByIdAsync(firebaseTokenAuthenticity.Uid);
                
                if(sqlUser == null)
                {
                    var firebaseUser = await _firebaseService.GetUserByIdAsync(firebaseTokenAuthenticity.Uid);
                    ApplicationUser newUser = new ApplicationUser 
                    {
                        Id = firebaseTokenAuthenticity.Uid,
                        FirstName = firebaseUser!.DisplayName,
                        DateJoined = DateTime.Now,
                        IsVerified = true,
                        IsPetSitter = false
                    };
                    UserProfile newUserProfile = new UserProfile {
                        //get access to other data from user's fb profile by using Facebook Graph API.
                        Id = firebaseTokenAuthenticity.Uid,
                        ProfilePictureUrl = firebaseUser.PhotoUrl
                    };

                    await _userRepository.AddAsync(newUser);
                    await _userRepository.AddUserProfile(newUserProfile);
                    await _userRepository.AddUserSettings(new UserSettings());
                }

                var roles = await _userRepository.GetRoles(firebaseTokenAuthenticity.Uid);
                var token = Security._Instance.GenerateJwtToken(sqlUser!,roles,_jwtSettings);

                response.JWToken = token;

                return response;
            }
            catch(Exception )
            {
                throw;
            }
        }
    }

}