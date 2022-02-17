using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using UniversityApi.Common;
using UniversityApi.Data;
using UniversityApi.Data.Repositories;
using UniversityApi.Entities.Contracts;
using UniversityApi.Services.Interfaces;
using UniversityApi.Services.Services;

namespace UniversityApi.WebFramework
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtService, JwtService>();

            
           
        }

        public static void AddDatabasecontext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
        }

        public static void AddJwtAuthentication(this IServiceCollection services,JwtSettings jwtSettings)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                byte[] secretKey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
                byte[] encryptKey = Encoding.UTF8.GetBytes(jwtSettings.EncryptionKey);

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.FromMinutes(jwtSettings.ClockSkew),
                    RequireSignedTokens = true,
                    
                    RequireExpirationTime = true,
                    ValidateLifetime = true,

                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),

                    TokenDecryptionKey = new SymmetricSecurityKey(encryptKey)
                };
            });
        }
    }
}
