using FurniNest_Backend.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FurniNest_Backend.Services.JwtServices
{
    public class JwtService:IJwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        public string? TokenGenerator(User user)
        {

            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claim = new[]
                {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.Role,user.Role),
                new Claim(ClaimTypes.Email,user.Email),
            };
                var token = new JwtSecurityToken(
                    claims: claim,
                    signingCredentials: credentials,
                    expires: DateTime.UtcNow.AddDays(1)
                    );
                return new JwtSecurityTokenHandler().WriteToken(token);

            }
        }
    }
}
