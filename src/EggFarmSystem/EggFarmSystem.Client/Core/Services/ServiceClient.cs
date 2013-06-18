using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using EggFarmSystem.Models;
using RestSharp;

namespace EggFarmSystem.Client.Core.Services
{
    public abstract class ServiceClient
    {
        private readonly IClientContext clientContext;
    
        protected ServiceClient(IClientContext clientContext)
        {
            this.clientContext = clientContext;
        }

        protected abstract string ResourceUrl { get; }

        protected T CreateGetRequest<T>(Guid id,string url = null) where T:new()
        {
            if (string.IsNullOrWhiteSpace(url))
                url = string.Format("{0}/{1}", ResourceUrl, id);

            var client = new RestClient(clientContext.ServiceUrl);
            var request = new RestRequest(url, Method.GET);
            AddHeaders(request);
            var response = client.Execute<T>(request);
            return response.Data;
        }

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

        protected bool CreatePostRequest<T>(T data, string url = null) where T : new()
        {
            if (string.IsNullOrWhiteSpace(url))
                url = ResourceUrl;

            var client = new RestClient(clientContext.ServiceUrl);
            var request = new RestRequest(url, Method.POST);
            AddHeaders(request);
            request.AddBody(data);
            var response = client.Execute(request);
            return response.StatusCode == HttpStatusCode.Created;
        }

        protected bool CreatePutRequest<T>(Guid id, T data, string url = null) where T : new()
        {
            if (string.IsNullOrWhiteSpace(url))
                url = string.Format("{0}/{1}", ResourceUrl, id);

            var client = new RestClient(clientContext.ServiceUrl);
            var request = new RestRequest(url, Method.PUT);
            AddHeaders(request);
            request.AddBody(data);
            var response = client.Execute(request);
            return response.StatusCode == HttpStatusCode.OK;
        }

        protected bool CreateDeleteRequest(Guid id, string url = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                url = string.Format("{0}/{1}", ResourceUrl, id);

            var client = new RestClient(clientContext.ServiceUrl);
            var request = new RestRequest(url, Method.DELETE);
            AddHeaders(request);
            var response = client.Execute(request);
            return response.StatusCode == HttpStatusCode.OK;
        }

        protected virtual void AddHeaders(RestRequest request)
        {
            string creds = String.Format("{0}:{1}", "anton", "anton");
            byte[] bytes = Encoding.ASCII.GetBytes(creds);
            request.AddHeader("Authorization", string.Format("Basic {0}", Convert.ToBase64String(bytes)));
            request.RequestFormat = DataFormat.Json; 
        }
    }
}
