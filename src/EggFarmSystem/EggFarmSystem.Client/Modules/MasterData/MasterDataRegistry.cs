using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using EggFarmSystem.Client.Modules.MasterData.Commands;
using EggFarmSystem.Client.Modules.MasterData.ViewModels;

namespace EggFarmSystem.Client.Modules.MasterData
{
    public class MasterDataRegistry : Autofac.Module
    {
        protected override void Load(Autofac.ContainerBuilder builder)
        {
            builder.RegisterType<ShowMasterDataCommand>();

            builder.RegisterType<MasterDataViewModel>().InstancePerDependency();
            builder.RegisterType<Views.View>().As<Views.IMasterDataView>().InstancePerDependency();

            
            base.Load(builder);
        }
    }
}
