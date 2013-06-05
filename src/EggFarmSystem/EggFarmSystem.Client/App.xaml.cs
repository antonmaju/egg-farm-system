using EggFarmSystem.Client.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EggFarmSystem.Client.Modules.MasterData;

namespace EggFarmSystem.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var bootstrapper = new Bootstrapper();
            bootstrapper.Register(new MasterDataModule());
            bootstrapper.Start(this);

            base.OnStartup(e);
        }
    }
}
