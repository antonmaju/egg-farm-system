using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Modules.Reports.Commands;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.Reports
{
    public class ReportsModule : IModule
    {
        public ReportsModule()
        {
            Registry = new ReportsRegistry();
            AvailableMenus = new List<MenuInfo>()
                {
                    new MenuInfo
                        {
                            CommandType = typeof(ShowReportListCommand),
                            Title =() => LanguageData.Reports_Title
                        }
                };
        }

        public string Name
        {
            get { return "Reports"; }
        }

        public IList<MenuInfo> AvailableMenus { get; private set; }

        public Autofac.Module Registry { get; private set; }

        public void Initialize()
        {
            
        }
    }
}
