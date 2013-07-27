using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Modules.EmployeeCost.Commands;
using EggFarmSystem.Client.Modules.EmployeeCost.ViewModels;
using EggFarmSystem.Resources;
using AutoMapper;

namespace EggFarmSystem.Client.Modules.EmployeeCost
{
    public class EmployeeCostModule : IModule
    {
        public EmployeeCostModule()
        {
            Registry = new EmployeeCostRegistry();

            AvailableMenus = new List<MenuInfo>()
                {
                    new MenuInfo
                        {
                            CommandType = typeof(ShowEmployeeCostCommand),
                            Title = () => LanguageData.EmployeeCost_Title
                        }
                };
        }
        
        public string Name
        {
            get { return "Employee Cost"; }
        }

        public IList<MenuInfo> AvailableMenus { get; private set; }

        public Autofac.Module Registry { get; private set; }

        public void Initialize()
        {
            InitializeMappings();
        }

        public void InitializeMappings()
        {
            Mapper.CreateMap<EmployeeCostDetailViewModel, Models.EmployeeCostDetail>();
            Mapper.CreateMap<Models.EmployeeCostDetail, EmployeeCostDetailViewModel>();
            Mapper.CreateMap<EmployeeCostEntryViewModel, Models.EmployeeCost>();
            Mapper.CreateMap<Models.EmployeeCost, EmployeeCostEntryViewModel>();
            Mapper.CreateMap<Models.EmployeeCostDetail, Models.Data.EmployeeCostDetail>();
            Mapper.CreateMap<Models.EmployeeCost, Models.Data.EmployeeCost>();
            Mapper.CreateMap<Models.Data.EmployeeCost, Models.EmployeeCost>();
            Mapper.CreateMap<Models.Data.EmployeeCostDetail, Models.EmployeeCostDetail>();
        }
    }
}
