using ElmahCore.Mvc;
using ElmahCore.Sql;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApi.Common;
using UniversityApi.WebFramework;
using UniversityApi.WebFramework.MiddleWares;

namespace UniversityApi
{
    public class Startup
    {
        private readonly SiteSettings _siteSettings;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //Reading From Appsettings.json section and cast to SiteSettings Class
            _siteSettings = configuration.GetSection("SiteSettings").Get<SiteSettings>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Register SiteSettings value for IOptionSnapShot which is placed in jwtService Constructor By DI
            services.Configure<SiteSettings>(Configuration.GetSection("SiteSettings"));

            services.AddControllers();
            services.AddJwtAuthentication(_siteSettings.JwtSettings);
            services.RegisterServices();
            services.AddDatabasecontext(Configuration);
            services.AddElmah<SqlErrorLog>(options =>
            {
                options.Path = "/elmah-errors";
                options.ConnectionString = Configuration.GetConnectionString("Elmah");
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UniversityApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<CustomExceptionHandlerMiddleware>();
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UniversityApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            
            app.UseAuthorization();
            
            app.UseElmah();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
