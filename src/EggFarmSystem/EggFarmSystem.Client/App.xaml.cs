using System.Globalization;
using System.Threading;
using System.Windows.Markup;
using EggFarmSystem.Client.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EggFarmSystem.Client.Modules.EggProduction;
using EggFarmSystem.Client.Modules.EmployeeCost;
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
            FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(
                CultureInfo.CurrentCulture.IetfLanguageTag)));

            var bootstrapper = new Bootstrapper();
            RegisterModules(bootstrapper);
            bootstrapper.Start(this);

            base.OnStartup(e);
        }

        private void RegisterModules(IBootstrapper bootstrapper)
        {
            bootstrapper.Register(new MasterDataModule());
            bootstrapper.Register(new ConsumableUsageModule());
            bootstrapper.Register(new EmployeeCostModule());
            bootstrapper.Register(new EggProductionModule());
        }
    }
}
