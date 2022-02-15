using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityApi.Entities.Models;
using UniversityApi.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace UniversityApi.Services.Services
{
    public class JwtService : IJwtService
    {
        public string GenerateToken(User user)
        {
            byte[] secretKey = Encoding.UTF8.GetBytes("MySecretKey123456789");
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),SecurityAlgorithms.HmacSha256Signature);
            var claims = GetClaims(user);
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = "MyWebSite",
                Audience = "MyWebsite",
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = signingCredentials,
                Subject =new ClaimsIdentity(claims)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken= tokenHandler.CreateToken(descriptor);
            string jwtToken = tokenHandler.WriteToken(securityToken);
            return jwtToken;
        }

        private IEnumerable<Claim> GetClaims(User user)
        {
            List<Claim> claimlst = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email)
            };
            Role[] roles = new Role[] { new Role { Name = user.Role.Name } };
            foreach (var role in roles)
            {
                claimlst.Add(new Claim(ClaimTypes.Role, role.Name));
            }
            return claimlst;

        }
    }
}
