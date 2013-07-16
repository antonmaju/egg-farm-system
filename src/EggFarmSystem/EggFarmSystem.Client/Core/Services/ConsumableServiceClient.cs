using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Models;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Core.Services
{
    public class ConsumableServiceClient : ServiceClient, IConsumableService
    {
        public ConsumableServiceClient(IClientContext clientContext) : base(clientContext)
        {
            
        }

        protected override string ResourceUrl
        {
            get { return "/consumables"; }
        }

        public IList<Consumable> GetAll()
        {
            return CreateGetAllRequest<List<Consumable>>();
        }

        public Consumable Get(Guid id)
        {
            return CreateGetRequest<Consumable>(id);
        }

        public void Save(Consumable model)
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
