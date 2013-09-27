using EggFarmSystem.Models;
using EggFarmSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Core.Services
{
    public class HenDepreciationServiceClient : ServiceClient, IHenDepreciationService
    {
        public HenDepreciationServiceClient(IClientContext context) : base(context)
        {

        }

        protected override string ResourceUrl
        {
            get { return "hendepreciation"; }
        }

        public Models.SearchResult<Models.HenDepreciation> Search(Models.DateRangeSearchInfo searchInfo)
        {
            string url = string.Format("{0}?page={1}&limit={2}&start={3}&end={4}",
               ResourceUrl, searchInfo.PageIndex, searchInfo.PageSize, searchInfo.Start, searchInfo.End);

            return CreateGetRequest<SearchResult<Models.HenDepreciation>>(Guid.Empty, url);
        }

        public Models.HenDepreciation Get(Guid id)
        {
            return CreateGetRequest<Models.HenDepreciation>(id);
        }

        public Models.HenDepreciation GetByDate(DateTime date)
        {
            var url = string.Format("{0}?date={1}", ResourceUrl, date.ToString("yyyy-MM-dd"));
            return CreateGetRequest<Models.HenDepreciation>(Guid.Empty, url);
        }

        public HenDepreciation GetInitialValues(DateTime date)
        {
            string url = string.Format("{0}/initialvalues/{1}", ResourceUrl, date.ToString("yyyy-MM-dd"));
            return CreateGetRequest<Models.HenDepreciation>(Guid.Empty, url);
        }

        public void Save(Models.HenDepreciation depreciation)
        {
            if (depreciation.IsNew)
                CreatePostRequest(depreciation);
            else
                CreatePutRequest(depreciation.Id, depreciation);
        }

        public void Delete(Guid id)
        {
            CreateDeleteRequest(id);
        }

    }
}
