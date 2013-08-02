using EggFarmSystem.Models;
using EggFarmSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Core.Services
{
    public class AdditionalCostServiceClient : ServiceClient, IAdditionalCostService
    {
        public AdditionalCostServiceClient(IClientContext clientContext) : base(clientContext)
        {
            
        }

        protected override string ResourceUrl
        {
            get { return "/additionalcost"; }
        }

        public IList<AdditionalCost> GetAll()
        {
            return CreateGetAllRequest<List<AdditionalCost>>();
        }

        public AdditionalCost Get(Guid id)
        {
            return CreateGetRequest<AdditionalCost>(id);
        }

        public void Save(AdditionalCost model)
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
