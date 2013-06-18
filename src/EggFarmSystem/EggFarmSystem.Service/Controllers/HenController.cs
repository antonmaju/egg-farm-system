using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using EggFarmSystem.Models;
using EggFarmSystem.Services;

namespace EggFarmSystem.Service.Controllers
{
    public class HenController : ApiController
    {
        private readonly IHenService henService;

        public HenController(IHenService henService)
        {
            this.henService = henService;
        }

        public IList<Hen> GetAll()
        {
            return null;
        }

        public Hen GetById(Guid id)
        {
            return null;
        }

        public HttpResponseMessage Post(Hen hen)
        {
            return null;
        }

        public void Put(Guid id, Hen hen)
        {
            
        }

        public void Delete(Guid id)
        {
            
        }
    }
}
