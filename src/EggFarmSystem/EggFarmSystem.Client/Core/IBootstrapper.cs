using System.Windows.Controls;
using EggFarmSystem.Client.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace EggFarmSystem.Client.Core
{
    public interface IBootstrapper
    {
        ICollection<IModule> Modules { get; }

        void Register(IModule module);

        void Start(Application app);

        IList<MenuItem> GetMainMenuItems();
    }
}
