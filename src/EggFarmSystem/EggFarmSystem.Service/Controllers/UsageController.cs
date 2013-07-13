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
    //TODO: probably need to refactor later
    public class UsageController : ApiControllerBase
    {
        private readonly IConsumableUsageService service;

        public UsageController(IConsumableUsageService service)
        {
            this.service = service;
        }

        public SearchResult<ConsumableUsage> GetByCriteria(int page, int limit, DateTime? start=null, DateTime? end=null)
        {
            var searchInfo = new ConsumableUsageSearchInfo
                {
                    Start = start,
                    End = end,
                    PageIndex = page,
                    PageSize = limit
                };

            return service.Search(searchInfo);
        }

        public ConsumableUsage Get(Guid id)
        {
            var usage = service.Get(id);
            if (usage == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return usage;
        }

        public ConsumableUsage GetByDate(DateTime date)
        {
            var usage = service.GetByDate(date);
            if (usage == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return usage;
        }

        public HttpResponseMessage Post(ConsumableUsage value)
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

        public HttpResponseMessage Put(Guid id, ConsumableUsage value)
        {
            ValidateModel(value);

            if (! ModelState.IsValid || value.Id != id)
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
            catch(Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }
    }
}
