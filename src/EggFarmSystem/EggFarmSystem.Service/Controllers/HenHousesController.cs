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

        public HttpResponseMessage Post(HenHouse value)
        {
            if(! ModelState.IsValid)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            HttpResponseMessage response = null;

            try
            {
                houseService.Save(value);
                response = Request.CreateResponse<HenHouse>(HttpStatusCode.Created, value);
                string uri = Url.Link("DefaultApi", new {id = value.Id});
                response.Headers.Location = new Uri(uri);
            }
            catch(Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

        [HttpPut]
        public HttpResponseMessage Put(Guid id,[FromBody] HenHouse value)
        {
            if (!ModelState.IsValid || value.Id != id)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            HttpResponseMessage response = null;

            try
            {
                houseService.Save(value);
                response = Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

        public HttpResponseMessage Delete(Guid id)
        {
            HttpResponseMessage response = null;

            try
            {
                houseService.Delete(id);
                response = Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }
    }
}
