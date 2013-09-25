using EggFarmSystem.Models;
using EggFarmSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace EggFarmSystem.Service.Controllers
{
    public class HenDepreciationController : ApiControllerBase
    {
        private readonly IHenDepreciationService service;

        public HenDepreciationController(IHenDepreciationService service)
        {
            this.service = service;
        }

        public SearchResult<HenDepreciation> GetByCriteria(int page, int limit, DateTime? start = null, DateTime? end = null)
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

        public HenDepreciation Get(Guid id)
        {
            var depreciation = service.Get(id);
            if (depreciation == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return depreciation;
        }

        public HenDepreciation GetByDate(DateTime date)
        {
            var cost = service.GetByDate(date);
            if (cost == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return cost;
        }

        public HenDepreciation GetInitialValues(DateTime date)
        {
            return service.GetInitialValues(date);
        }

        public HttpResponseMessage Post(HenDepreciation value)
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

        public HttpResponseMessage Put(Guid id, HenDepreciation value)
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
