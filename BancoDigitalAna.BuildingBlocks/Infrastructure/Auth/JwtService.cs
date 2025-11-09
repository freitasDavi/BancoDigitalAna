using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BancoDigitalAna.BuildingBlocks.Infrastructure.Auth
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public JwtService(IOptions<JwtSettings> settings)
        {
            _jwtSettings = settings.Value;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public string GenerateToken(string contaId, string numeroConta, string cpf)
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var signingCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                );

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, contaId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("numeroConta", numeroConta),
                new Claim("cpf", cpf),
                new Claim(JwtRegisteredClaimNames.Iat, 
                    DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = signingCredentials,
            };

            var token = _jwtSecurityTokenHandler.CreateToken(tokenDescriptor);

            return _jwtSecurityTokenHandler.WriteToken(token);
        }

        public TokenValidationResult Validation(string token)
        {
            try
            {
                var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };

                var principal = _jwtSecurityTokenHandler.ValidateToken(
                    token,
                    validationParameters,
                    out var validatedToken
                    );

                return new TokenValidationResult
                {
                    IsValid = true,
                    ContaId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value,
                    NumeroConta = principal.FindFirst("numeroConta")?.Value,
                    Cpf = principal.FindFirst("cpf")?.Value
                };
            } catch (SecurityTokenExpiredException)
            {
                return new TokenValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Token expirado"
                };
            }
            catch (SecurityTokenException ex)
            {
                return new TokenValidationResult
                {
                    IsValid = false,
                    ErrorMessage = $"Token inválido: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new TokenValidationResult
                {
                    IsValid = false,
                    ErrorMessage = $"Erro ao validar token: {ex.Message}"
                };
            }
        }
    }
}
