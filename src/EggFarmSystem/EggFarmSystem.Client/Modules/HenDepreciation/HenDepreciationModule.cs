using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Resources;
using EggFarmSystem.Client.Modules.HenDepreciation.Commands;

namespace EggFarmSystem.Client.Modules.HenDepreciation
{
    public class HenDepreciationModule : IModule
    {
        public HenDepreciationModule()
        {
            Registry = new HenDepreciationRegistry();

            AvailableMenus = new List<MenuInfo>()
                {
                     new MenuInfo
                        {
                            CommandType = typeof(ShowHenDepreciationListCommand),
                            Title = () => LanguageData.HenDepreciation_Title
                        }
                };
        }

        public string Name
        {
            get { return "Hen Depreciation"; }
        }

        public IList<MenuInfo> AvailableMenus { get; private set; }

        public Autofac.Module Registry { get; private set; }

        public void Initialize()
        {
            
        }
    }
}
