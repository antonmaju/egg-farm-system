using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Models;
using EggFarmSystem.Services;
using RestSharp;

namespace EggFarmSystem.Client.Core.Services
{
    public class HenHouseServiceClient : ServiceClient, IHenHouseService
    {
        public HenHouseServiceClient(IClientContext clientContext) : base(clientContext)
        {
            
        }

        protected override string ResourceUrl
        {
            get { return "/henhouses"; }
        }

        public IList<HenHouse> GetAll()
        {
            return CreateGetAllRequest<List<HenHouse>>();
        }

        public HenHouse Get(Guid id)
        {
            return CreateGetRequest<HenHouse>(id);
        }

        public int GetPopulation(Guid houseId)
        {
            string url = string.Format("{0}/{1}/population",
                 ResourceUrl, houseId.ToString());
            return CreateGetRequest<int>(Guid.Empty, url, true);
        }

        public void Save(Models.HenHouse model)
        {
            if (model.IsNew)
                CreatePostRequest(model);
            else
                CreatePutRequest(model.Id, model);
        }

        public void Delete(Guid id)
        {
            CreateDeleteRequest(id);
        }
    }
}
