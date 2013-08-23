using System.Net;
using EggFarmSystem.Models;
using EggFarmSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace EggFarmSystem.Service.Controllers
{
    public class HenHousesController : ApiControllerBase<HenHouse>
    {
        private IHenHouseService houseService;

        public HenHousesController(IHenHouseService houseService) : base(houseService)
        {
            this.houseService = houseService;
        }   
    
        public int GetPopulation(Guid id)
        {
            return houseService.GetPopulation(id);
        }
    }
}
