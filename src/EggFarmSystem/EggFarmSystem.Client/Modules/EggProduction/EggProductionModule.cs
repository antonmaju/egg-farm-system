using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using EggFarmSystem.Client.Modules.EggProduction.Commands;
using EggFarmSystem.Client.Modules.EggProduction.ViewModels;
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
                            CommandType = typeof(ShowEggProductionListCommand),
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
            Mapper.CreateMap<EggProductionDetailViewModel, Models.EggProductionDetail>();
            Mapper.CreateMap<Models.EggProductionDetail, EggProductionDetailViewModel>();
            Mapper.CreateMap<EggProductionEntryViewModel, Models.EggProduction>();
            Mapper.CreateMap<Models.EggProduction, EggProductionEntryViewModel>();
            Mapper.CreateMap<Models.EggProductionDetail, Models.Data.EggProductionDetail>();
            Mapper.CreateMap<Models.EggProduction, Models.Data.EggProduction>();
            Mapper.CreateMap<Models.Data.EggProduction, Models.EggProduction>();
            Mapper.CreateMap<Models.Data.EggProductionDetail, Models.EggProductionDetail>();
        }
    }
}
