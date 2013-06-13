using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using EggFarmSystem.Services;
using ServiceStack.OrmLite;
using System.Configuration;

namespace EggFarmSystem.Client.Core
{
    public class ServiceClientModule : Module
    {
        public bool IsDirectAccess { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
           RegisterDirectAccess(builder);

            base.Load(builder);
        }

        void RegisterDirectAccess(ContainerBuilder builder)
        {
            builder.RegisterInstance(
                new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["EggFarmDb"].ConnectionString,
                                             SqlServerDialect.Provider))
                   .As<IDbConnectionFactory>();

            builder.RegisterType<HenService>().As<IHenService>().SingleInstance();
            builder.RegisterType<HenHouseService>().As<IHenHouseService>().SingleInstance();
            builder.RegisterType<EmployeeService>().As<IEmployeeService>().SingleInstance();

        }
    }
}
