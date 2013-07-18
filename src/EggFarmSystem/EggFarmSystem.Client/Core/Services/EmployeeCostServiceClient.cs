using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Models;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Core.Services
{
    public class EmployeeCostServiceClient : ServiceClient, IEmployeeCostService
    {
        public EmployeeCostServiceClient(IClientContext clientContext) : base(clientContext)
        {
            
        }

        protected override string ResourceUrl
        {
            get { return "/employeecost"; }
        }

        public SearchResult<EmployeeCost> Search(DateRangeSearchInfo searchInfo)
        {
            string url = string.Format("{0}?page={1}&limit={2}&start={3}&end={4}",
                ResourceUrl, searchInfo.PageIndex, searchInfo.PageSize, searchInfo.Start, searchInfo.End);

            return CreateGetRequest<SearchResult<EmployeeCost>>(Guid.Empty, url);
        }

        public EmployeeCost Get(Guid id)
        {
            return CreateGetRequest<EmployeeCost>(id);
        }

        public Models.EmployeeCost GetByDate(DateTime date)
        {
            var url = string.Format("{0}?date={1}", ResourceUrl, date.ToString("yyyy-MM-dd"));
            return CreateGetRequest<EmployeeCost>(Guid.Empty, url);
        }

        public void Save(Models.EmployeeCost cost)
        {
            if (cost.IsNew)
                base.CreatePostRequest(cost);
            else
                base.CreatePutRequest(cost.Id, cost);
        }

        public void Delete(Guid id)
        {
            CreateDeleteRequest(id);
        }
    }
}
