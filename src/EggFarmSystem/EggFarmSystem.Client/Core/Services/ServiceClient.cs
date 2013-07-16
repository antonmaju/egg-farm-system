using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using EggFarmSystem.Models;
using RestSharp;

namespace EggFarmSystem.Client.Core.Services
{
    /// <summary>
    /// Base class for REST service client
    /// </summary>
    public abstract class ServiceClient
    {
        private readonly IClientContext clientContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceClient"/> class.
        /// </summary>
        /// <param name="clientContext">The client context.</param>
        protected ServiceClient(IClientContext clientContext)
        {
            this.clientContext = clientContext;
        }

        /// <summary>
        /// Gets the resource URL.
        /// </summary>
        /// <value>The resource URL.</value>
        protected abstract string ResourceUrl { get; }

        /// <summary>
        /// Creates the get request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The id.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        protected T CreateGetRequest<T>(Guid id, string url = null) where T:new()
        {
            if (string.IsNullOrWhiteSpace(url))
                url = string.Format("{0}/{1}", ResourceUrl, id);


            var client = new RestClient(clientContext.ServiceUrl);
            var request = new RestRequest(url, Method.GET);
            AddHeaders(request);
            var response = client.Execute<T>(request);
            return response.Data;
        }

        /// <summary>
        /// Creates the get all request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        protected T CreateGetAllRequest<T>(string url = null) where T : new()
        {
            if (string.IsNullOrWhiteSpace(url))
                url = ResourceUrl;

            var client = new RestClient(clientContext.ServiceUrl);
            var request = new RestRequest(url, Method.GET);
            AddHeaders(request);
            var response = client.Execute<T>(request);
            return response.Data;
        }

        /// <summary>
        /// Creates the post request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <param name="url">The URL.</param>
        /// <returns><c>true</c> if post operation success, <c>false</c> otherwise</returns>
        protected void CreatePostRequest<T>(T data, string url = null) where T : new()
        {
            if (string.IsNullOrWhiteSpace(url))
                url = ResourceUrl;

            var client = new RestClient(clientContext.ServiceUrl);
            var request = new RestRequest(url, Method.POST);
            AddHeaders(request);
            request.AddBody(data);
            var response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.Created)
            {
                throw new ServiceException(response.ErrorMessage);       
            }
        }

        /// <summary>
        /// Creates the put request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The id.</param>
        /// <param name="data">The data.</param>
        /// <param name="url">The URL.</param>
        protected void CreatePutRequest<T>(Guid id, T data, string url = null) where T : new()
        {
            if (string.IsNullOrWhiteSpace(url))
                url = string.Format("{0}/{1}", ResourceUrl, id);

            var client = new RestClient(clientContext.ServiceUrl);
            var request = new RestRequest(url, Method.PUT);
            AddHeaders(request);
            request.AddBody(data);
            var response = client.Execute(request);
            if(response.StatusCode != HttpStatusCode.OK)
                throw new ServiceException(response.ErrorMessage);
        }

        /// <summary>
        /// Creates the delete request.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="url">The URL.</param>
        /// <returns><c>true</c> if delete success, <c>false</c> otherwise</returns>
        protected void CreateDeleteRequest(Guid id, string url = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                url = string.Format("{0}/{1}", ResourceUrl, id);

            var client = new RestClient(clientContext.ServiceUrl);
            var request = new RestRequest(url, Method.DELETE);
            AddHeaders(request);
            var response = client.Execute(request);

            if(response.StatusCode != HttpStatusCode.OK)
                throw new ServiceException(response.ErrorMessage);
        }

        /// <summary>
        /// Adds the headers.
        /// </summary>
        /// <param name="request">The request.</param>
        protected virtual void AddHeaders(RestRequest request)
        {
            string creds = String.Format("{0}:{1}", "anton", "anton");
            byte[] bytes = Encoding.ASCII.GetBytes(creds);
            request.AddHeader("Authorization", string.Format("Basic {0}", Convert.ToBase64String(bytes)));
            request.RequestFormat = DataFormat.Json; 
        }
    }
}
