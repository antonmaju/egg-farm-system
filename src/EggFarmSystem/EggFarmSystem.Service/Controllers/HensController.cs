using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using EggFarmSystem.Models;
using EggFarmSystem.Services;

namespace EggFarmSystem.Service.Controllers
{
    public class HensController : ApiControllerBase<Hen>
    {
        public HensController(IHenService henService) : base(henService)
        {
        }


    }
}
