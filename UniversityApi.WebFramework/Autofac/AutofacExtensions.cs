using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityApi.Common;
using UniversityApi.Data;
using UniversityApi.Data.Repositories;
using UniversityApi.Entities.Common;
using UniversityApi.Entities.Contracts;
using UniversityApi.Services.Services;

namespace UniversityApi.WebFramework.Autofac
{
   public static class AutofacExtensions
    {
        public static void AddServices(this ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            var commonAssembly = typeof(SiteSettings).Assembly;
            var enititiesAssembly = typeof(IEntity).Assembly;
            var dataAssembly = typeof(DatabaseContext).Assembly;
            var serviceAssembly = typeof(JwtService).Assembly;

            builder.RegisterAssemblyTypes(commonAssembly,enititiesAssembly,dataAssembly, serviceAssembly)
                   .AssignableTo<IScopedDependency>()
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(commonAssembly, enititiesAssembly, dataAssembly, serviceAssembly)
                   .AssignableTo<ISingletoneDependency>()
                   .AsImplementedInterfaces()
                   .SingleInstance();

            builder.RegisterAssemblyTypes(commonAssembly, enititiesAssembly, dataAssembly, serviceAssembly)
                   .AssignableTo<ITransientDependency>()
                   .AsImplementedInterfaces()
                   .InstancePerDependency();
        }
    }
}
