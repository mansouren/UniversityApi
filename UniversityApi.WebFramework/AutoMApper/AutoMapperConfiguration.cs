using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UniversityApi.Services.Dtos;
using UniversityApi.Services.Dtos.Common;

namespace UniversityApi.WebFramework.AutoMApper
{
   public static class AutoMapperConfiguration
    {
        public static void InitializeAutoMapper(this IServiceCollection services,params Assembly[] assemblies)
        {
            services.AddAutoMapper(config =>
            {
                config.AddCustomMappingProfile();
            }, assemblies);
        }

        public static void AddCustomMappingProfile(this IMapperConfigurationExpression config)
        {
            var serviceAssembly = typeof(IHaveCustomMapping).Assembly;
            config.AddCustomMappingProfile(serviceAssembly);
        }

        public static void AddCustomMappingProfile(this IMapperConfigurationExpression config,params Assembly[] assemblies)
        {
            var allTypes = assemblies.SelectMany(a => a.ExportedTypes);

            var list = allTypes.Where(type => type.IsClass && !type.IsAbstract
            && type.GetInterfaces().Contains(typeof(IHaveCustomMapping)))
                .Select(type => (IHaveCustomMapping)Activator.CreateInstance(type));

            var profile = new CustomMappingProfile(list);
            config.AddProfile(profile);
        }
    }
}
