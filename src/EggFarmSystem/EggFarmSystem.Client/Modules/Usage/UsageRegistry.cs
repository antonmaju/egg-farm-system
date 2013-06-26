﻿using Autofac;
using EggFarmSystem.Client.Modules.Usage.Commands;
using EggFarmSystem.Client.Modules.Usage.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.Usage
{
    public class UsageRegistry : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterCommands(builder);
            RegisterViewModels(builder);
            RegisterViews(builder);

            base.Load(builder);
        }

        private void RegisterViewModels(ContainerBuilder builder)
        {
            builder.RegisterType<UsageListViewModel>().SingleInstance();
        }

        private void RegisterViews(ContainerBuilder builder)
        {
            
        }

        private void RegisterCommands(ContainerBuilder builder)
        {
            builder.RegisterType<NewUsageCommand>().SingleInstance();
            builder.RegisterType<EditUsageCommand>().SingleInstance();
            builder.RegisterType<DeleteUsageCommand>().SingleInstance();
        }
    }
}