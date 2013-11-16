using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Commands;
using EggFarmSystem.Client.Modules.MasterData.ViewModels;
using EggFarmSystem.Models;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.MasterData
{
    public class MasterDataModule : IModule
    {
        public MasterDataModule()
        {
            Registry = new MasterDataRegistry();
            AvailableMenus = new List<MenuInfo>()
                {
                    new MenuInfo
                        {
                            CommandType = typeof (ShowMasterDataCommand),
                            Title = () => LanguageData.Master_Title.Replace(" ","\n")
                        }
                };
        }

        public string Name
        {
            get { return "Master Data"; }
        }

        public IList<MenuInfo> AvailableMenus { get; private set; }

        public Autofac.Module Registry { get; private set; }

        public void Initialize()
        {
            RegisterMappings();
        }

        void RegisterMappings()
        {
            Mapper.CreateMap<AdditionalCostEntryViewModel, AdditionalCost>();
            Mapper.CreateMap<HenEntryViewModel, Hen>();
            Mapper.CreateMap<HouseEntryViewModel,HenHouse>();
            Mapper.CreateMap<EmployeeEntryViewModel, Employee>();
            Mapper.CreateMap<ConsumableEntryViewModel, Consumable>();
            Mapper.CreateMap<Hen, HenListItem>();
        }
    }
}
