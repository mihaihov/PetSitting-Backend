using MediatR;
using Microsoft.Extensions.Options;
using PetSitting.Application.Exceptions.Firebase;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Application.Interfaces.Services;
using PetSitting.Domain.Entities.UserManagement;
using PetSitting.Domain.Entities.Utils;
using PetSitting.Domain.Features;

namespace PetSitting.Application.Features.UserManagement.Commands
{
    public class LoginWithGoogleCommandHandler : BaseHandler<UserManagementBaseCommand<ThirdPartyAuthResponse>, ThirdPartyAuthResponse>
    {
        private readonly IFirebaseService _firebaseService;
        private readonly IUserRepository _userRepository;
        private readonly IOptions<JwtSettings> _jwtSettings;
        public LoginWithGoogleCommandHandler(IFirebaseService firebaseService, IUserRepository userRepository,
            IOptions<JwtSettings> jwtSettings)
        {
            _firebaseService = firebaseService;
            _userRepository = userRepository;
            _jwtSettings = jwtSettings;
        }

        protected override async Task<ThirdPartyAuthResponse> HandleCommand(UserManagementBaseCommand<ThirdPartyAuthResponse> request, ThirdPartyAuthResponse response, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.FirebaseToken)) throw new Exception("Something went wrong!");

            //verifies firebase token agains the firebase database
            var firebaseTokenAuthenticity = await _firebaseService.VerifyTokenAsync(request.FirebaseToken);
            if (firebaseTokenAuthenticity == null)
                throw new FirebaseUserNotFoundException();

            //verifies if user exists in sql db; if it does not, creat it.
            var sqlUser = await _userRepository.GetByIdAsync(firebaseTokenAuthenticity.Uid);

            if (sqlUser == null)
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
                UserProfile newUserProfile = new UserProfile
                {
                    //get access to other data from user's fb profile by using Google API.
                    ApplicationUserId = firebaseTokenAuthenticity.Uid,
                    ProfilePictureUrl = firebaseUser.PhotoUrl
                };

                await _userRepository.AddAsync(newUser);
                await _userRepository.AddUserProfile(newUserProfile);
                await _userRepository.AddUserSettings(new UserSettings());
            }

            var roles = await _userRepository.GetRoles(firebaseTokenAuthenticity.Uid);

            response.JWToken = Security._Instance.GenerateJwtToken(sqlUser!, roles, _jwtSettings);
            var refreshToken = Security._Instance.GenerateRefreshToken(sqlUser!, roles, _jwtSettings);
            response.RefreshsToken = refreshToken.Token;
            await _userRepository.StoreRefreshToken(refreshToken);

            return response;

        }
    }
}
