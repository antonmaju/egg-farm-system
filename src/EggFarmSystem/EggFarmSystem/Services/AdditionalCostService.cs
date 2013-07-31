using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Models;
using ServiceStack.OrmLite;

namespace EggFarmSystem.Services
{
    public interface IAdditionalCostService : IDataService<AdditionalCost>
    {
        
    }

    public class AdditionalCostService : DataService<AdditionalCost>, IAdditionalCostService
    {
        public AdditionalCostService(IDbConnectionFactory factory) : base(factory)
        {

        }
    }
}
