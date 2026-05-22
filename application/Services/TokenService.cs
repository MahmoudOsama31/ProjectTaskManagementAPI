using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectTaskManagement.Models;
using ProjectTaskManagement.Options;

namespace ProjectTaskManagement.Services;

public class TokenService : ServiceBase, ITokenService
{
    private readonly JwtOptions _jwt;

    public TokenService(IOptions<JwtOptions> jwtOptions, ILogger<TokenService> logger)
        : base(logger)
    {
        _jwt = jwtOptions.Value;
    }

    public string CreateToken(User user) =>
        Execute(() =>
        {
            if (string.IsNullOrWhiteSpace(_jwt.Key))
            {
                throw new InvalidOperationException("JWT signing key is not configured.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.ExpireMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        });
}
