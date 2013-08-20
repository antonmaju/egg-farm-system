using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using EggFarmSystem.Client.Modules.EggProduction.Commands;
using EggFarmSystem.Client.Modules.EggProduction.ViewModels;
using EggFarmSystem.Client.Modules.EggProduction.Views;

namespace EggFarmSystem.Client.Modules.EggProduction
{
    public class EggProductionRegistry : Module
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
            builder.RegisterType<DeleteEggProductionCommand>().SingleInstance();
            builder.RegisterType<EditEggProductionCommand>().SingleInstance();
            builder.RegisterType<NewEggProductionCommand>().SingleInstance();
            builder.RegisterType<SaveEggProductionCommand>().SingleInstance();
            builder.RegisterType<ShowEggProductionCommand>().SingleInstance();
        }

        void RegisterViewModels(ContainerBuilder builder)
        {
            builder.RegisterType<EggProductionListViewModel>().InstancePerDependency();
        }

        void RegisterViews(ContainerBuilder builder)
        {
            builder.RegisterType<EggProductionListView>().As<IEggProductionListView>().InstancePerDependency();
        }
    }
}
