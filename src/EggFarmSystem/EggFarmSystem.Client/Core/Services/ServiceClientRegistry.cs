using System.Configuration;
using Autofac;
using EggFarmSystem.Services;
using ServiceStack.OrmLite;

namespace EggFarmSystem.Client.Core.Services
{
    /// <summary>
    /// Contains autofac type registration service clients
    /// </summary>
    public class ServiceClientRegistry : Module
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is direct db access or REST service access.
        /// </summary>
        /// <value><c>true</c> if this instance is direct access; otherwise, <c>false</c>.</value>
        public bool IsDirectAccess { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            if (IsDirectAccess)
                RegisterDirectAccess(builder);
            else
                RegisterServiceAccess(builder);
            
            base.Load(builder);
        }

        /// <summary>
        /// Registers the direct access services.
        /// </summary>
        /// <param name="builder">The builder.</param>
        void RegisterDirectAccess(ContainerBuilder builder)
        {
            builder.RegisterInstance(
                new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["EggFarmDb"].ConnectionString,
                                             MySqlDialect.Provider))
                   .As<IDbConnectionFactory>();

            builder.RegisterType<HenService>().As<IHenService>().SingleInstance();
            builder.RegisterType<HenHouseService>().As<IHenHouseService>().SingleInstance();
            builder.RegisterType<EmployeeService>().As<IEmployeeService>().SingleInstance();
            builder.RegisterType<ConsumableService>().As<IConsumableService>().SingleInstance();
            builder.RegisterType<ConsumableUsageService>().As<IConsumableUsageService>().SingleInstance();
            builder.RegisterType<EmployeeCostService>().As<IEmployeeCostService>().SingleInstance();
            builder.RegisterType<AdditionalCostService>().As<IAdditionalCostService>().SingleInstance();
            builder.RegisterType<EggProductionService>().As<IEggProductionService>().SingleInstance();
            builder.RegisterType<HenDepreciationService>().As<IHenDepreciationService>().SingleInstance();
            builder.RegisterType<ReportingService>().As<IReportingService>().SingleInstance();
        }

        /// <summary>
        /// Registers the REST service access.
        /// </summary>
        /// <param name="builder">The builder.</param>
        void RegisterServiceAccess(ContainerBuilder builder)
        {
            builder.RegisterType<HenServiceClient>().As<IHenService>().SingleInstance();
            builder.RegisterType<HenHouseServiceClient>().As<IHenHouseService>().SingleInstance();
            builder.RegisterType<EmployeeServiceClient>().As<IEmployeeService>().SingleInstance();
            builder.RegisterType<ConsumableServiceClient>().As<IConsumableService>().SingleInstance();
            builder.RegisterType<ConsumableUsageServiceClient>().As<IConsumableUsageService>().SingleInstance();
            builder.RegisterType<EmployeeCostServiceClient>().As<IEmployeeCostService>().SingleInstance();
            builder.RegisterType<AdditionalCostServiceClient>().As<IAdditionalCostService>().SingleInstance();
            builder.RegisterType<EggProductionServiceClient>().As<IEggProductionService>().SingleInstance();
            builder.RegisterType<HenDepreciationServiceClient>().As<IHenDepreciationService>().SingleInstance();
            builder.RegisterType<ReportingServiceClient>().As<IReportingService>().SingleInstance();
        }
    }
}
