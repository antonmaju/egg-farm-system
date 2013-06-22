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
            
        }
    }
}
