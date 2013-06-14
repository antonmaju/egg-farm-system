using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using EggFarmSystem.Client.Modules.MasterData.Commands;
using EggFarmSystem.Client.Modules.MasterData.ViewModels;
using EggFarmSystem.Client.Modules.MasterData.Views;

namespace EggFarmSystem.Client.Modules.MasterData
{
    public class MasterDataRegistry : Autofac.Module
    {
        protected override void Load(Autofac.ContainerBuilder builder)
        {
            RegisterCommands(builder);

            RegisterViewModels(builder);

            RegisterViews(builder);
          
            base.Load(builder);
        }

        void RegisterCommands(ContainerBuilder builder)
        {
            builder.RegisterType<ShowMasterDataCommand>().SingleInstance();
            builder.RegisterType<NewHenCommand>().SingleInstance();
            builder.RegisterType<EditHenCommand>().SingleInstance();
            builder.RegisterType<SaveHenCommand>().SingleInstance();
            builder.RegisterType<DeleteHenCommand>().SingleInstance();
            
            builder.RegisterType<NewHouseCommand>().SingleInstance();
            builder.RegisterType<EditHouseCommand>().SingleInstance();
            builder.RegisterType<DeleteHouseCommand>().SingleInstance();
            builder.RegisterType<SaveHenHouseCommand>().SingleInstance();

            builder.RegisterType<NewEmployeeCommand>().SingleInstance();
            builder.RegisterType<EditEmployeeCommand>().SingleInstance();
            builder.RegisterType<DeleteEmployeeCommand>().SingleInstance();
            builder.RegisterType<SaveEmployeeCommand>().SingleInstance();

            builder.RegisterType<NewConsumableCommand>().SingleInstance();
            builder.RegisterType<EditConsumableCommand>().SingleInstance();
            builder.RegisterType<DeleteConsumableCommand>().SingleInstance();
            builder.RegisterType<SaveConsumableCommand>().SingleInstance();
        }

        void RegisterViewModels(ContainerBuilder builder)
        {
            builder.RegisterType<MasterDataViewModel>().InstancePerDependency();
            builder.RegisterType<HenListViewModel>().InstancePerDependency();
            builder.RegisterType<HenEntryViewModel>().InstancePerDependency();
            builder.RegisterType<HouseListViewModel>().InstancePerDependency();
            builder.RegisterType<HouseEntryViewModel>().InstancePerDependency();
            builder.RegisterType<EmployeeListViewModel>().InstancePerDependency();
            builder.RegisterType<EmployeeEntryViewModel>().InstancePerDependency();
            builder.RegisterType<ConsumableListViewModel>().InstancePerDependency();
            builder.RegisterType<ConsumableEntryViewModel>().InstancePerDependency();
        }

        void RegisterViews(ContainerBuilder builder)
        {
            builder.RegisterType<View>().As<IMasterDataView>().InstancePerDependency();
            builder.RegisterType<EmployeeEntryView>().As<IEmployeeEntryView>().InstancePerDependency();
            builder.RegisterType<EmployeeListView>().As<IEmployeeListView>().InstancePerDependency();
            builder.RegisterType<HenEntryView>().As<IHenEntryView>().InstancePerDependency();
            builder.RegisterType<HenListView>().As<IHenListView>().InstancePerDependency();
            builder.RegisterType<HenHouseEntryView>().As<IHenHouseEntryView>().InstancePerDependency();
            builder.RegisterType<HenHouseListView>().As<IHenHouseListView>().InstancePerDependency();
            builder.RegisterType<ConsumableListView>().As<IConsumableListView>().InstancePerDependency();
            builder.RegisterType<ConsumableEntryView>().As<IConsumableEntryView>().InstancePerDependency();
        }
    }
}
