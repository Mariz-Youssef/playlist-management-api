using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PlaylistManagement.Api.Features.Authentication.DTOs;
using PlaylistManagement.Api.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PlaylistManagement.Api.Features.Authentication
{
    public class TokenService: ITokenService
    {
        private readonly JwtOptions _options;
        public TokenService(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public AuthResponse GenerateAccessToken(User user)
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes);
           
            var claims = new List<Claim>
            {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Email, user.Email)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
           
            var token = new JwtSecurityToken(
               issuer: _options.Issuer,
               audience: _options.Audience,
               claims: claims,
               expires: expiresAt,
               signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return new AuthResponse
            {
                AccessToken = accessToken,
                ExpiresAt = expiresAt
            };
        }
    }
}
