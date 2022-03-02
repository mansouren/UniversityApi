using Common;
using ElmahCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UniversityApi.Common;
using UniversityApi.Common.Exceptions;
using UniversityApi.Common.Utilities;
using UniversityApi.Data;
using UniversityApi.Data.Repositories;
using UniversityApi.Entities.Contracts;
using UniversityApi.Entities.Models;
using UniversityApi.Services.Interfaces;
using UniversityApi.Services.Services;

namespace UniversityApi.WebFramework
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
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

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        //var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                        //logger.LogError("Authentication failed.", context.Exception);
                        if (context.Exception != null)
                            throw new AppException(ApiResultStatusCode.UnAuthorized, "Authentication failed.", HttpStatusCode.Unauthorized, context.Exception, null);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = async context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();

                        //Invoking Claims that stored in claims in jwtService
                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                        if (claimsIdentity.Claims?.Any() != true)
                            context.Fail("Token Has No Claim!");

                        var securityStamp = claimsIdentity.FindFirstValue(new ClaimsIdentityOptions().SecurityStampClaimType);
                        if (!securityStamp.HasValue())
                            context.Fail("This token has no security stamp");
                      
                        //int userid =claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value.ToInt();
                        
                        int userId = claimsIdentity.GetUserId<int>();
                        User user =await userService.GetUserById(userId, context.HttpContext.RequestAborted);
                        
                        if (user.SecurityStamp != Guid.Parse(securityStamp))
                            context.Fail("Token Security Stamp Is Not Valid!");

                        if (!user.IsActive)
                            context.Fail("User is Not Active!");

                        await userService.UpdateLastLoginDate(user, context.HttpContext.RequestAborted);
                    }
                };
            });
        }
    }
} 
