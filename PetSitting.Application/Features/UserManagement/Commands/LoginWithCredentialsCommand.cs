using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FirebaseAdmin.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetSitting.Application.Common;
using PetSitting.Application.Features.UserManagement.Validators;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Application.Interfaces.Services;
using PetSitting.Domain.Entities.UserManagement;
using PetSitting.Domain.Entities.Utils;

namespace PetSitting.Application.Features.UserManagement
{
    public record LoginWithCredentialsCommand(string email, string password) : IRequest<LoginWithCredentialsCommandResponse>;
    public record LoginWithCredentialsCommandResponse : BaseResponse
    {
        public string? JWToken {get;set;} = null;
        public string? RefreshsToken {get;set;} = null;
    }

    public class LoginWithCredentialsCommandHandler : IRequestHandler<LoginWithCredentialsCommand, LoginWithCredentialsCommandResponse>
    {
        private readonly IFirebaseServices _firebaseService;
        private readonly IUserRepository _userRepository;
        private readonly IOptions<JwtSettings> _jwtSettings;
        public LoginWithCredentialsCommandHandler(IFirebaseServices firebaseService, IUserRepository userRepository,
            IOptions<JwtSettings> jwtSettings)
        {
            _firebaseService = firebaseService;
            _userRepository = userRepository;
            _jwtSettings = jwtSettings;
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

                var loginResult = await _firebaseService.SignInWithEmailAndPasswordAsync(request.email,request.password);
                if(loginResult == null)
                    throw new Exception("LogIn failed");
                var tokenVerification = await _firebaseService.VerifyTokenAsync(loginResult.FirebaseToken);

                var sqlUser = await _userRepository.GetByIdAsync(tokenVerification.Uid);
                if(sqlUser == null)
                    throw new Exception ("User not authorized");

                var roles = await _userRepository.GetRoles(sqlUser.Id.ToString());

                response.JWToken = GenerateJwtToken(sqlUser,roles);
                response.RefreshsToken = loginResult.RefreshToken;

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GenerateJwtToken(ApplicationUser user, IReadOnlyList<string> roles)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim("uid", user.Id) // Firebase UID
            };

            // Add roles as claims
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Value.Issuer,
                audience: _jwtSettings.Value.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.Value.ExpiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}