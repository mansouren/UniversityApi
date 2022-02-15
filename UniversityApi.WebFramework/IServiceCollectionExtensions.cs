using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using UniversityApi.Data;
using UniversityApi.Data.Repositories;
using UniversityApi.Entities.Contracts;
using UniversityApi.Services.Interfaces;
using UniversityApi.Services.Services;

namespace UniversityApi.WebFramework
{
   public static class IServiceCollectionExtensions
    {
        public static void RegisterServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("sqlConnection")));

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            //});

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtService, JwtService>();
        }
    }
}
