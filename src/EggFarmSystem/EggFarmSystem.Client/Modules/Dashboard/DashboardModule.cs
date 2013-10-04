using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Modules.Dashboard.Commands;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.Dashboard
{
    public class DashboardModule : IModule
    {
        public DashboardModule()
        {
            Registry = new DashboardRegistry();

            AvailableMenus = new List<MenuInfo>()
                {
                    new MenuInfo
                        {
                            CommandType = typeof(ShowDashboardCommand),
                            Title = ()=> LanguageData.Dashboard_Title
                        }
                };
        }

        public string Name
        {
            get { return "Dashboard"; }
        }

        public IList<MenuInfo> AvailableMenus { get; private set; }

        public Autofac.Module Registry { get; private set; }

        public void Initialize()
        {
            
        }
    }
}
