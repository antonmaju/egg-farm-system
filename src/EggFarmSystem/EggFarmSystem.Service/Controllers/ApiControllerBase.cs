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
    public abstract class ApiControllerBase<T> : ApiController where T:Entity
    {
        private readonly IDataService<T> service;

        protected ApiControllerBase(IDataService<T> service)
        {
            this.service = service;
        }

        protected void ValidateModel(T entity)
        {
            if (entity == null) return;

            var errors = entity.Validate();

            if (errors == null || errors.Count == 0) return;

            foreach (var modelError in errors)
            {
                ModelState.AddModelError(modelError.PropertyName, modelError.Message);
            }
        }

        protected IList<ErrorInfo> GetModelErrors()
        {
            var errors = new List<ErrorInfo>();

            foreach (var modelState in ModelState)
            {
                var key = modelState.Key;

                foreach (var error in modelState.Value.Errors)
                {
                    errors.Add(new ErrorInfo(key, error.ErrorMessage));
                }
            }

            return errors;
        }

        public virtual IList<T> GetAll()
        {
            return service.GetAll();
        }

        public virtual T GetById(Guid id)
        {
            var entity = service.Get(id);

            if(entity == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return entity;
        }

        public virtual HttpResponseMessage Post(T value)
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
                response = Request.CreateResponse<T>(HttpStatusCode.Created, value);
                string uri = Url.Link("DefaultApi", new { id = value.Id });
                response.Headers.Location = new Uri(uri);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

        public virtual HttpResponseMessage Put(Guid id, T value)
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

        public virtual HttpResponseMessage Delete(Guid id)
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
