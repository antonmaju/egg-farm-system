using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using EggFarmSystem.Client.Modules.HenDepreciation.Commands;

namespace EggFarmSystem.Client.Modules.HenDepreciation
{
    public class HenDepreciationRegistry : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterCommands(builder);

            base.Load(builder);
        }

        void RegisterCommands(ContainerBuilder builder)
        {
            builder.RegisterType<DeleteHenDepreciationCommand>().SingleInstance();
            builder.RegisterType<EditHenDepreciationCommand>().SingleInstance();
            builder.RegisterType<NewHenDepreciationCommand>().SingleInstance();
            builder.RegisterType<SaveHenDepreciationCommand>().SingleInstance();
            builder.RegisterType<ShowHenDepreciationCommand>().SingleInstance();
        }
    }
}
