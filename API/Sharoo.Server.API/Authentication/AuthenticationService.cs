using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sharoo.Server.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sharoo.Server.API.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly string? _jwtSecret;

        public AuthenticationService(IOptions<JwtSettings> jwtOptions)
        {
            _jwtSecret = jwtOptions.Value.Secret;

            if (string.IsNullOrEmpty(_jwtSecret)) throw new ArgumentNullException("Ocorreu um erro durante o processamento.");
        }

        public string GenerateToken(User user)
        {
            var handler = new JwtSecurityTokenHandler();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));

            var token = handler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(user),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            });

            return handler.WriteToken(token);
        }

        private ClaimsIdentity GenerateClaims(User user)
        {
            ClaimsIdentity claims = new();
            claims.AddClaim(new Claim("Id", user.Id.ToString()));
            claims.AddClaim(new Claim(ClaimTypes.Name, user.Name));
            claims.AddClaim(new Claim(ClaimTypes.Email, user.Email));

            foreach (var role in user.Role)
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
    }
}
