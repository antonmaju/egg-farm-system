using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using EggFarmSystem.Client.Modules.EggProduction.Commands;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.EggProduction
{
    public class EggProductionModule : IModule
    {
        public EggProductionModule()
        {
            Registry = new EggProductionRegistry();

            AvailableMenus = new List<MenuInfo>()
                {
                    new MenuInfo
                        {
                            CommandType = typeof(ShowEggProductionCommand),
                            Title = () => LanguageData.EggProduction_Title
                        }
                };
        }

        public string Name
        {
            get { return "Egg Production"; }
        }

        public IList<MenuInfo> AvailableMenus { get; private set; }

        public Autofac.Module Registry { get; private set; }

        public void Initialize()
        {
            InitializeMappings();
        }

        public void InitializeMappings()
        {
            
        }
    }
}
