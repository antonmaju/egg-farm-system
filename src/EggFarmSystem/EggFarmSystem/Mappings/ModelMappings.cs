using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace EggFarmSystem.Mappings
{
    public class ModelMappings : IMappingRegistration
    {
        public void Initialize()
        {
            Mapper.CreateMap<Models.ConsumableUsage, Models.Data.ConsumableUsage>();
            Mapper.CreateMap<Models.ConsumableUsageDetail, Models.Data.ConsumableUsageDetail>();
            Mapper.CreateMap<Models.Data.ConsumableUsage, Models.ConsumableUsage>();
            Mapper.CreateMap<Models.Data.ConsumableUsageDetail, Models.ConsumableUsageDetail>();

            Mapper.CreateMap<Models.EmployeeCost, Models.Data.EmployeeCost>();
            Mapper.CreateMap<Models.EmployeeCostDetail, Models.Data.EmployeeCostDetail>();
            Mapper.CreateMap<Models.Data.EmployeeCost, Models.EmployeeCost>();
            Mapper.CreateMap<Models.Data.EmployeeCostDetail, Models.EmployeeCostDetail>();

            Mapper.CreateMap<Models.Data.EggProduction, Models.EggProduction>();
            Mapper.CreateMap<Models.Data.EggProductionDetail, Models.EggProductionDetail>();
            Mapper.CreateMap<Models.EggProductionDetail, Models.Data.EggProductionDetail>();
            Mapper.CreateMap<Models.EggProduction, Models.Data.EggProduction>();

            Mapper.CreateMap<Models.Data.HenDepreciation, Models.HenDepreciation>();
            Mapper.CreateMap<Models.Data.HenDepreciationDetail, Models.HenDepreciationDetail>();
            Mapper.CreateMap<Models.HenDepreciationDetail, Models.Data.HenDepreciationDetail>();
            Mapper.CreateMap<Models.HenDepreciation, Models.Data.HenDepreciation>();
        }
    }
}
