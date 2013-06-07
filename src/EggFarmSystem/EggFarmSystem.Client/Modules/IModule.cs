using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using EggFarmSystem.Client.Core;

namespace EggFarmSystem.Client.Modules
{
    public interface IModule
    {
        string Name { get; }

        IList<MenuInfo> AvailableMenus { get; }

        Autofac.Module Registry { get; }

        void RegisterMessageSubscriber(IMessageBroker broker);

        
    }
}
