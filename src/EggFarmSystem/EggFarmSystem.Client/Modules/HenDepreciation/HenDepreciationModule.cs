using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using EggFarmSystem.Client.Modules.HenDepreciation.ViewModels;
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
                            Title = () => LanguageData.HenDepreciation_Title.Replace(" ", "\n")
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
            InitializeMappings();
        }

        void InitializeMappings()
        {
            Mapper.CreateMap<HenDepreciationEntryViewModel, Models.HenDepreciation>();
            Mapper.CreateMap<Models.HenDepreciation, HenDepreciationEntryViewModel>();
            Mapper.CreateMap<HenDepreciationDetailViewModel, Models.HenDepreciationDetail>();
            Mapper.CreateMap<Models.HenDepreciationDetail, HenDepreciationDetailViewModel>();
            Mapper.CreateMap<Models.HenDepreciation, Models.Data.HenDepreciation>();
            Mapper.CreateMap<Models.Data.HenDepreciation, Models.HenDepreciation>();
            Mapper.CreateMap<Models.HenDepreciationDetail, Models.Data.HenDepreciationDetail>();
            Mapper.CreateMap<Models.Data.HenDepreciationDetail, Models.HenDepreciationDetail>();
        }
    }
}
