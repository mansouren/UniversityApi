using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
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

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IUserService, UserService>();
        }
    }
}
