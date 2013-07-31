using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Models;
using EggFarmSystem.Services;

namespace EggFarmSystem.Service.Controllers
{
    public class AdditionalCostController : ApiControllerBase<AdditionalCost>
    {
        public AdditionalCostController(IAdditionalCostService service) : base(service)
        {
            
        }
    }
}
