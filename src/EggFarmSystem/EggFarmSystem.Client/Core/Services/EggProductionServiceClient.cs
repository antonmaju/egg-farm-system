using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Models;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Core.Services
{
    public class EggProductionServiceClient : ServiceClient, IEggProductionService
    {
        public EggProductionServiceClient(IClientContext clientContext):base(clientContext)
        {
            
        }

        protected override string ResourceUrl
        {
            get { return "eggproduction"; }
        }

        public Models.SearchResult<Models.EggProduction> Search(Models.DateRangeSearchInfo searchInfo)
        {
            string url = string.Format("{0}?page={1}&limit={2}&start={3}&end={4}",
                ResourceUrl, searchInfo.PageIndex, searchInfo.PageSize, searchInfo.Start, searchInfo.End);

            return CreateGetRequest<SearchResult<Models.EggProduction>>(Guid.Empty, url);
        }

        public Models.EggProduction Get(Guid id)
        {
            return CreateGetRequest<Models.EggProduction>(id);
        }

        public Models.EggProduction GetByDate(DateTime date)
        {
            var url = string.Format("{0}?date={1}", ResourceUrl, date.ToString("yyyy-MM-dd"));
            return CreateGetRequest<Models.EggProduction>(Guid.Empty, url);
        }

        public void Save(Models.EggProduction production)
        {
            if (production.IsNew)
                CreatePostRequest(production);
            else
                CreatePutRequest(production.Id, production);
        }

        public void Delete(Guid id)
        {
            CreateDeleteRequest(id);
        }
    }
}
