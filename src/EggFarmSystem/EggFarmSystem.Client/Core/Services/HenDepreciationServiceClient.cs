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
            throw new NotImplementedException();
        }

        public Models.HenDepreciation Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Models.HenDepreciation GetByDate(DateTime date)
        {
            throw new NotImplementedException();
        }

        public void Save(Models.HenDepreciation depreciation)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
