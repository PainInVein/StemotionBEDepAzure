using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using STEMotion.Application.Interfaces.ServiceInterfaces;
using STEMotion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Services
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _configuration;
        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(Guid userId, string email, string role)
        {
            var jwtKey = _configuration["JWT:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var duration = double.Parse(_configuration["Jwt:DurationInMinutes"]);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()), 
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim("Role", role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
              issuer: issuer,
              audience: audience,
              claims: claims,
              expires: DateTime.Now.AddMinutes(duration),
              signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,        
                ValidateLifetime = true,         
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
                )
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
    }
}
