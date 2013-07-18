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
    public class EmployeeCostController : ApiControllerBase
    {
        private readonly IEmployeeCostService service;

        public EmployeeCostController(IEmployeeCostService service)
        {
            this.service = service;
        }

        public SearchResult<EmployeeCost> GetByCriteria(int page, int limit, DateTime? start = null, DateTime? end = null)
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

        public EmployeeCost Get(Guid id)
        {
            var cost = service.Get(id);
            if(cost == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return cost;
        }

        public EmployeeCost GetByDate(DateTime date)
        {
            var cost = service.GetByDate(date);
            if(cost == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return cost;
        }

        public HttpResponseMessage Post(EmployeeCost value)
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

        public HttpResponseMessage Put(Guid id, EmployeeCost value)
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
