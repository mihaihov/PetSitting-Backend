
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetSitting.Domain.Entities.Security;
using PetSitting.Domain.Entities.UserManagement;
using PetSitting.Domain.Entities.Utils;

namespace PetSitting.Domain.Features
{
    public class Security
    {
        #pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        private static Security _instance = null;
        #pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        
        public static Security _Instance {
            get{
                if(_instance == null) _instance = new Security();
                return _instance;
            }
        }

        private Security() { }

        public string GenerateJwtToken(ApplicationUser user, IReadOnlyList<string> roles, IOptions<JwtSettings> _jwtSettings)
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

        public RefreshToken GenerateRefreshToken(ApplicationUser user, IReadOnlyList<string> roles, IOptions<JwtSettings> _jwtSettings)
        {
            RefreshToken refreshToken = new RefreshToken {
                Token = GenerateJwtToken(user,roles,_jwtSettings),
                UserId = user.Id,
                User = user
            };
            return refreshToken;
        }
    }
}