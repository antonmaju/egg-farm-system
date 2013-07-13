using Autofac;
using EggFarmSystem.Services;
using ServiceStack.OrmLite;
using System.Configuration;
namespace EggFarmSystem.Service.Core.Installers
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(
               new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["EggFarmDb"].ConnectionString,
                                            MySqlDialect.Provider))
                  .As<IDbConnectionFactory>();

            builder.RegisterType<HenService>().As<IHenService>().SingleInstance();
            builder.RegisterType<HenHouseService>().As<IHenHouseService>().SingleInstance();
            builder.RegisterType<EmployeeService>().As<IEmployeeService>().SingleInstance();
            builder.RegisterType<ConsumableService>().As<IConsumableService>().SingleInstance();
            builder.RegisterType<AccountService>().As<IAccountService>().SingleInstance();
            builder.RegisterType<ConsumableUsageService>().As<IConsumableUsageService>().SingleInstance();
            base.Load(builder);
        }
    }
}
