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
    public class HenHousesController : ApiController
    {
        private readonly IHenHouseService houseService;

        public HenHousesController(IHenHouseService houseService)
        {
            this.houseService = houseService;
        }

        public IList<HenHouse> GetAll()
        {
            return houseService.GetAll();
        }

        public HenHouse GetById(Guid id)
        {
            var house = houseService.Get(id);

            if(house == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return house;
        }

        public HttpResponseMessage Post(HenHouse house)
        {
            if(! ModelState.IsValid)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            houseService.Save(house);
            var response = Request.CreateResponse<HenHouse>(HttpStatusCode.Created, house);
            string uri = Url.Link("DefaultApi", new { id = house.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        public void Put(Guid id,HenHouse house)
        {
            if (!ModelState.IsValid || house.Id != id)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            
            houseService.Save(house);
        }

        public void Delete(Guid id)
        {
            houseService.Delete(id);
        }
    }
}
