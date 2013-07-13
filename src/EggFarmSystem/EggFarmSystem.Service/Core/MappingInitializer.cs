using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AutoMapper;

namespace EggFarmSystem.Service.Core
{
    public static class MappingInitializer
    {
        /// <summary>
        /// Initializes AutoMapper mapping
        /// </summary>
        public static void Initialize()
        {
            Mapper.CreateMap<Models.ConsumableUsage, Models.Data.ConsumableUsage>();
                  
            Mapper.CreateMap<Models.ConsumableUsageDetail, Models.Data.ConsumableUsageDetail>();
        }
    }
}
