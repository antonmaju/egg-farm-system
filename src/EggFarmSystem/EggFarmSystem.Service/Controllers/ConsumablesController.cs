using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Models;
using EggFarmSystem.Services;

namespace EggFarmSystem.Service.Controllers
{
    public class ConsumablesController : ApiControllerBase<Consumable>
    {
        public ConsumablesController(IConsumableService service) : base(service)
        {
            
        }
    }
}
