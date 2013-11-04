using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using EggFarmSystem.Client.Modules.Reports.Commands;
using EggFarmSystem.Client.Modules.Reports.ViewModels;
using EggFarmSystem.Client.Modules.Reports.Views;

namespace EggFarmSystem.Client.Modules.Reports
{
    public class ReportsRegistry : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterCommands(builder);
            RegisterViewModels(builder);
            RegisterModels(builder);

            base.Load(builder);
        }

        void RegisterCommands(ContainerBuilder builder)
        {
            builder.RegisterType<ShowReportListCommand>().SingleInstance();
        }

        void RegisterViewModels(ContainerBuilder builder)
        {
            builder.RegisterType<ReportListViewModel>().InstancePerDependency();

            builder.RegisterType<EmployeeCostReportViewModel>().InstancePerDependency();
<<<<<<< HEAD

            builder.RegisterType<UsageReportViewModel>().InstancePerDependency();
=======
            builder.RegisterType<EggProductionReportViewModel>().InstancePerDependency();
>>>>>>> remotes/upstream/master
        }

        void RegisterModels(ContainerBuilder builder)
        {
            builder.RegisterType<ReportListView>().As<IReportListView>().InstancePerDependency();

            builder.RegisterType<EmployeeCostReportView>().As<IEmployeeCostReportView>().InstancePerDependency();
<<<<<<< HEAD

            builder.RegisterType<UsageReportView>().As<IUsageReportView>().InstancePerDependency();
=======
            builder.RegisterType<EggProductionReportView>().As<IEggProductionReportView>().InstancePerDependency();
>>>>>>> remotes/upstream/master
        }
    }
}
