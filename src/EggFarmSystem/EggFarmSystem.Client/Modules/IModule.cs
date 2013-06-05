using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace EggFarmSystem.Client.Modules
{
    public interface IModule
    {
        string Name { get; }

        IList<MenuInfo> AvailableMenus { get; }

        Autofac.Module Registry { get; }  
    }
}
