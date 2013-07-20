using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using EggFarmSystem.Client.Modules.EmployeeCost.Commands;
using EggFarmSystem.Client.Modules.EmployeeCost.ViewModels;
using EggFarmSystem.Client.Modules.EmployeeCost.Views;

namespace EggFarmSystem.Client.Modules.EmployeeCost
{
    public class EmployeeCostRegistry : Module
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
            builder.RegisterType<DeleteEmployeeCostCommand>().SingleInstance();
            builder.RegisterType<EditEmployeeCostCommand>().SingleInstance();
            builder.RegisterType<NewEmployeeCostCommand>().SingleInstance();
            builder.RegisterType<SaveEmployeeCostCommand>().SingleInstance();
            builder.RegisterType<ShowEmployeeCostCommand>().SingleInstance();
            }

        void RegisterViewModels(ContainerBuilder builder)
        {
            builder.RegisterType<EmployeeCostListViewModel>().InstancePerDependency();
        }

        void RegisterViews(ContainerBuilder builder)
        {
            builder.RegisterType<EmployeeCostListView>().As<IEmployeeCostListView>().InstancePerDependency();
        }
    }
}
