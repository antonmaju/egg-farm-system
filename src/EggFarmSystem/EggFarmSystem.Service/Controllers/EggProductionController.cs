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
    public class EggProductionController : ApiControllerBase
    {
        private readonly IEggProductionService service;

        public EggProductionController(IEggProductionService service)
        {
            this.service = service;
        }

        public SearchResult<EggProduction> GetByCriteria(int page, int limit, DateTime? start = null, DateTime? end = null)
        {
            var searchInfo = new DateRangeSearchInfo
            {
                Start = start,
                End = end,
                PageIndex = page,
                PageSize = limit
            };

            return service.Search(searchInfo);
        }

        public EggProduction Get(Guid id)
        {
            var production = service.Get(id);
            if (production == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return production;
        }

        public EggProduction GetByDate(DateTime date)
        {
            var cost = service.GetByDate(date);
            if (cost == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return cost;
        }

        public HttpResponseMessage Post(EggProduction value)
        {
            HttpResponseMessage response = null;
            ValidateModel(value);

            if (!ModelState.IsValid)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, GetModelErrors());
                return response;
            }

            try
            {
                service.Save(value);
                response = Request.CreateResponse(HttpStatusCode.Created, value);
                string uri = Url.Link("DefaultApi", new { id = value.Id });
                response.Headers.Location = new Uri(uri);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

        public HttpResponseMessage Put(Guid id, EggProduction value)
        {
            ValidateModel(value);

            if (!ModelState.IsValid || value.Id != id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, GetModelErrors());
            }

            HttpResponseMessage response = null;

            try
            {
                service.Save(value);
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
                service.Delete(id);
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
