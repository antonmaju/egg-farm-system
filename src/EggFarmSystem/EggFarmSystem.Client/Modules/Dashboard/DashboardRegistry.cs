using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using EggFarmSystem.Client.Modules.Dashboard.Commands;
using EggFarmSystem.Client.Modules.Dashboard.ViewModels;
using EggFarmSystem.Client.Modules.Dashboard.Views;

namespace EggFarmSystem.Client.Modules.Dashboard
{
    public class DashboardRegistry : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterCommands(builder);
            RegisterViewModels(builder);
            RegisterViews(builder);

            base.Load(builder);
        }

        void RegisterViewModels(ContainerBuilder builder)
        {
            builder.RegisterType<DashboardViewModel>().InstancePerDependency();
            builder.RegisterType<ProgressStageViewModel>().InstancePerDependency();
        }

        void RegisterViews(ContainerBuilder builder)
        {
            builder.RegisterType<DashboardView>().As<IDashboardView>().InstancePerDependency();
            builder.RegisterType<ProgressStageView>().As<IProgressStageView>().InstancePerDependency();
        }

        void RegisterCommands(ContainerBuilder builder)
        {
            builder.RegisterType<ShowDashboardCommand>().SingleInstance();
        }
    }
}
