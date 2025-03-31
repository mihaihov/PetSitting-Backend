using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FirebaseAdmin.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Commands;
using PetSitting.Application.Features.UserManagement.Validators;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Application.Interfaces.Services;
using PetSitting.Domain.Entities.Security;
using PetSitting.Domain.Entities.UserManagement;
using PetSitting.Domain.Entities.Utils;
using PetSitting.Domain.Features;
using PetSitting.Application.Features.UserManagement.Entities;

namespace PetSitting.Application.Features.UserManagement
{
    public record LoginWithCredentialsCommand(string email, string password) : IRequest<LoginWithCredentialsCommandResponse>;
    public record LoginWithCredentialsCommandResponse : BaseResponse
    {
        public string? JWToken {get;set;} = null;
        public string? RefreshsToken {get;set;} = null;

        //more properties later.
    }

    public class LoginWithCredentialsCommandHandler : BaseCommandHandler<LoginWithCredentialsCommand,LoginWithCredentialsCommandResponse,LoginWithCredentialsCommandValidator>
    {
        private readonly IFirebaseService _firebaseService;
        private readonly IUserRepository _userRepository;
        private readonly IOptions<JwtSettings> _jwtSettings;
        public LoginWithCredentialsCommandHandler(IFirebaseService firebaseService, IUserRepository userRepository,
            IOptions<JwtSettings> jwtSettings)
        {
            _firebaseService = firebaseService;
            _userRepository = userRepository;
            _jwtSettings = jwtSettings;
        }

        protected override async Task<LoginWithCredentialsCommandResponse> HandleCommand(LoginWithCredentialsCommand request, LoginWithCredentialsCommandResponse response, CancellationToken cancellationToken)
        {
            try
            {
                var loginResult = await _firebaseService.SignInWithEmailAndPasswordAsync(request.email,request.password);
                if(loginResult == null)
                    throw new Exception("LogIn failed");
                var tokenVerification = await _firebaseService.VerifyTokenAsync(loginResult.FirebaseToken);

                var sqlUser = await _userRepository.GetByIdAsync(tokenVerification.Uid);
                if(sqlUser == null)
                    throw new Exception ("User not authorized");

                var roles = await _userRepository.GetRoles(sqlUser.Id.ToString());

                response.JWToken = Security._Instance.GenerateJwtToken(sqlUser,roles, _jwtSettings);
                var refreshToken = Security._Instance.GenerateRefreshToken(sqlUser,roles,_jwtSettings);
                response.RefreshsToken = refreshToken.Token;

                await _userRepository.StoreRefreshToken(refreshToken);


                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}