using System.Configuration;
using Autofac;
using EggFarmSystem.Services;
using ServiceStack.OrmLite;

namespace EggFarmSystem.Client.Core.Services
{
    public class ServiceClientRegistry : Module
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
                                             MySqlDialect.Provider))
                   .As<IDbConnectionFactory>();

            builder.RegisterType<HenServiceClient>().As<IHenService>().SingleInstance();
            builder.RegisterType<HenHouseServiceClient>().As<IHenHouseService>().SingleInstance();
            builder.RegisterType<EmployeeServiceClient>().As<IEmployeeService>().SingleInstance();
            builder.RegisterType<ConsumableServiceClient>().As<IConsumableService>().SingleInstance();

        }
    }
}
