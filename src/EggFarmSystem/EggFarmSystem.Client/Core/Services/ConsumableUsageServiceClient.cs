using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Core.Services
{
    public class ConsumableUsageServiceClient : ServiceClient, IConsumableUsageService 
    {
        public ConsumableUsageServiceClient(IClientContext context):base(context)
        {
            
        }

        protected override string ResourceUrl
        {
            get { throw new NotImplementedException(); }
        }


        public Models.SearchResult<Models.ConsumableUsage> Search(Models.ConsumableUsageSearchInfo searchInfo)
        {
            throw new NotImplementedException();
        }

        public Models.ConsumableUsage Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Models.ConsumableUsage GetByDate(DateTime date)
        {
            throw new NotImplementedException();
        }

        public bool Save(Models.ConsumableUsage usage)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }

       
    }
}
