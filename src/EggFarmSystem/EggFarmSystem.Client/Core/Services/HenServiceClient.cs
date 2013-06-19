using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Models;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Core.Services
{
    public class HenServiceClient : ServiceClient, IHenService
    {
        public HenServiceClient(IClientContext clientContext) : base(clientContext)
        {
            
        }

        protected override string ResourceUrl
        {
            get { return "/hens"; }
        }

        public IList<Hen> GetAll()
        {
            return CreateGetAllRequest<List<Hen>>();
        }

        public Hen Get(Guid id)
        {
            return CreateGetRequest<Hen>(id);
        }

        public bool Save(Models.Hen model)
        {
            return model.Id == Guid.Empty ? CreatePostRequest(model) : CreatePutRequest(model.Id, model);
        }

        public bool Delete(Guid id)
        {
            return CreateDeleteRequest(id);
        }

        public SearchResult<Hen> Search()
        {
            throw new NotImplementedException();
        }
    }
}
