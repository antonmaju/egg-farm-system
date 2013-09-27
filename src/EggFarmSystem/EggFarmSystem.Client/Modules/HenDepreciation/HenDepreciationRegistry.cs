using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using EggFarmSystem.Client.Modules.HenDepreciation.Commands;
using EggFarmSystem.Client.Modules.HenDepreciation.ViewModels;
using EggFarmSystem.Client.Modules.HenDepreciation.Views;

namespace EggFarmSystem.Client.Modules.HenDepreciation
{
    public class HenDepreciationRegistry : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterCommands(builder);
            RegisterViewModels(builder);
            RegisterViews(builder);

            base.Load(builder);
        }

        void RegisterCommands(ContainerBuilder builder)
        {
            builder.RegisterType<DeleteHenDepreciationCommand>().SingleInstance();
            builder.RegisterType<EditHenDepreciationCommand>().SingleInstance();
            builder.RegisterType<NewHenDepreciationCommand>().SingleInstance();
            builder.RegisterType<SaveHenDepreciationCommand>().SingleInstance();
            builder.RegisterType<ShowHenDepreciationListCommand>().SingleInstance();
        }

        void RegisterViewModels(ContainerBuilder builder)
        {
            builder.RegisterType<HenDepreciationListViewModel>().InstancePerDependency();
            builder.RegisterType<HenDepreciationEntryViewModel>().InstancePerDependency();
        }

        void RegisterViews(ContainerBuilder builder)
        {
            builder.RegisterType<HenDepreciationListView>().As<IHenDepreciationListView>().InstancePerDependency();
            builder.RegisterType<HenDepreciationEntryView>().As<IHenDepreciationEntryView>().InstancePerDependency();
        }
    }
}
