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

        public bool Save(Models.HenHouse model)
        {
            return model.Id == Guid.Empty ? CreatePostRequest(model) : CreatePutRequest(model.Id,model);
        }

        public bool Delete(Guid id)
        {
            return CreateDeleteRequest(id);
        }

    }
}
