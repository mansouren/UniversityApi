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
using Microsoft.Extensions.Options;
using UniversityApi.Common;

namespace UniversityApi.Services.Services
{
    public class JwtService : IJwtService
    {
        private readonly SiteSettings _settings;
        public JwtService(IOptionsSnapshot<SiteSettings> settings)
        {
            _settings = settings.Value;
        }
        public string GenerateToken(User user)
        {
            byte[] secretKey = Encoding.UTF8.GetBytes(_settings.JwtSettings.SecretKey);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),SecurityAlgorithms.HmacSha256Signature);

            byte[] encryptKey = Encoding.UTF8.GetBytes(_settings.JwtSettings.EncryptionKey);
            var encryptionCredential = new EncryptingCredentials(new SymmetricSecurityKey(encryptKey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);
            
            var claims = GetClaims(user);
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _settings.JwtSettings.Issuer,
                Audience = _settings.JwtSettings.Audience,
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(_settings.JwtSettings.NotBeforeMinutes),
                Expires = DateTime.Now.AddMinutes(_settings.JwtSettings.ExpirationMinutes),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptionCredential,
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
