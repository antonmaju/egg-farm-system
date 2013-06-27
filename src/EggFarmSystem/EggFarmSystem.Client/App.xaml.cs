using System.Globalization;
using System.Threading;
using EggFarmSystem.Client.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EggFarmSystem.Client.Modules.MasterData;
using EggFarmSystem.Client.Modules.Usage;

namespace EggFarmSystem.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("id-ID");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("id-ID");

            var bootstrapper = new Bootstrapper();
            bootstrapper.Register(new MasterDataModule());
            bootstrapper.Register(new ConsumableUsageModule());
            bootstrapper.Start(this);

            base.OnStartup(e);
        }
    }
}
