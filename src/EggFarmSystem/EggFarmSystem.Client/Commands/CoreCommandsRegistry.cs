using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace EggFarmSystem.Client.Commands
{
    public class CoreCommandsRegistry : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CancelCommand>().SingleInstance();
            base.Load(builder);
        }
    }
}
