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

        public bool Save(Consumable model)
        {
            return model.IsNew ? CreatePostRequest(model) : CreatePutRequest(model.Id, model);
        }

        public bool Delete(Guid id)
        {
            return CreateDeleteRequest(id);
        }
    }
}
