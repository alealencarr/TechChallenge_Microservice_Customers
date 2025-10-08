using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Entities.Aggregates.AggregateUser;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Composition;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;
        private readonly string _issuer;
        private readonly string _audience;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!));
            _issuer = _configuration["Jwt:Issuer"]!;
            _audience = _configuration["Jwt:Audience"]!;
        }

        public (string AccessToken, string RefreshToken) GenerateAdminTokens(User user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Name),
                new(JwtRegisteredClaimNames.Email, user.Mail),
                new(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
            };

            foreach (var userRole in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
            }

            var accessToken = GenerateToken(claims, DateTime.UtcNow.AddMinutes(15)); 
            var refreshToken = GenerateRefreshToken();
 
            return (accessToken, refreshToken);
        }

        public string GenerateCustomerToken(Customer customer)
        {
            var claims = new List<Claim>
            {
                new("cpf", customer.Cpf!.Valor),
                new(ClaimTypes.Name, customer.Name ?? string.Empty),
                new(JwtRegisteredClaimNames.Email, customer.Mail ?? string.Empty),
                new(ClaimTypes.Role, "Customer")
            };

            return GenerateToken(claims, DateTime.UtcNow.AddHours(1));  
        }

        public string GenerateGuestToken()
        {
            var claims = new List<Claim>
            {
                // Um ID único para a sessão anônima
                new("guest_id", Guid.NewGuid().ToString()),
                new(ClaimTypes.Role, "Guest")
            };

            return GenerateToken(claims, DateTime.UtcNow.AddHours(1)); // Mesma duração do cliente
        }

        /// <summary>
        /// Gera uma string criptograficamente segura para ser usada como Refresh Token.
        /// </summary>
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }


        // Método privado para evitar duplicação de código
        private string GenerateToken(IEnumerable<Claim> claims, DateTime expires)
        {
 
            var token = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                expires: expires,
                signingCredentials: new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature)
            );

              return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
