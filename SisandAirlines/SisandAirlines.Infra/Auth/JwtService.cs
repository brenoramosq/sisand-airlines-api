using Microsoft.IdentityModel.Tokens;
using SisandAirlines.Domain.Entities;
using SisandAirlines.Domain.Interfaces.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace SisandAirlines.Infra.Auth
{
    public class JwtService : IJwtService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtService(string secretKey, string issuer, string audience) 
        {
            _secretKey = secretKey;
            _issuer = issuer;
            _audience = audience;
        }

        public string GenerateToken(Customer customer)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var resources = new List<string> { "user" };
            string resourcesJson = JsonSerializer.Serialize(resources);

            var claims = new List<Claim>
            {
                new Claim("email", customer.Email),
                new Claim("nome", customer.FullName),
                new Claim("resources", resourcesJson),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken
            (
                 issuer: _issuer,
                 audience: _audience,
                 claims: claims,
                 expires: DateTime.Now.AddHours(1),
                 signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
