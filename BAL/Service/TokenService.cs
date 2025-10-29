using BAL.Interface;
using Core.Models.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BAL.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IConfiguration configuration, ILogger<TokenService> logger)
        {
            _config = configuration;
            _logger = logger;

        }
        public async Task<TokenResponse> GenerateTokenAsync(TokenResquest tokenResquest)
        {
            var userName = tokenResquest.userName ?? string.Empty;
            var claims = new List<Claim> { new(ClaimTypes.Name, userName) };

            var token = CreateToken(claims);

            var tokenString = await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
            _logger.LogInformation("Token Generate successful <Token>: {token}", tokenString);

            return new TokenResponse
            {
                Token = tokenString,
                Username = userName,
                IssuedAt = token.ValidFrom,
                ExpiresAt = token.ValidTo
            };
        }

        public JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var secretKey = _config["JWT:SecretKey"];
            if (!string.IsNullOrEmpty(secretKey))
            {
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                _ = int.TryParse(_config["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

                var token = new JwtSecurityToken(
                    issuer: _config["JWT:Issuer"],
                    audience: _config["JWT:Audience"],
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return token;
            }
            return new();
        }

    }
}
