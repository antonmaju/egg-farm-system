using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Models;
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
            get { return "/usage"; }
        }


        public Models.SearchResult<Models.ConsumableUsage> Search(Models.ConsumableUsageSearchInfo searchInfo)
        {
            string url = string.Format("{0}?page={1}&limit={2}&start={3}&end={4}",
                ResourceUrl ,searchInfo.PageIndex, searchInfo.PageSize, searchInfo.Start, searchInfo.End);

            return CreateGetRequest<SearchResult<ConsumableUsage>>(Guid.Empty, url);
        }

        public Models.ConsumableUsage Get(Guid id)
        {
            return CreateGetRequest<ConsumableUsage>(id);
        }

        public Models.ConsumableUsage GetByDate(DateTime date)
        {
            var url = string.Format("{0}?date={1}", ResourceUrl, date.ToString("yyyy-MM-dd"));
            return CreateGetRequest<ConsumableUsage>(Guid.Empty, url);
        }

        public void Save(Models.ConsumableUsage model)
        {
            if(model.IsNew)
                base.CreatePostRequest(model);
            else
                base.CreatePutRequest(model.Id, model);
        }

        public void Delete(Guid id)
        {
            CreateDeleteRequest(id);
        }
    }
}
