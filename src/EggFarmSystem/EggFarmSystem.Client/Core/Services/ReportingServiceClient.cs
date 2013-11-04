using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Models.Reporting;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Core.Services
{
    public class ReportingServiceClient : ServiceClient, IReportingService
    {
        public ReportingServiceClient(IClientContext context) : base(context)
        {
            
        }

        protected override string ResourceUrl
        {
            get { return "report"; }
        }

        public IList<EmployeeCostSummary> GetEmployeeCostSummary(DateTime start, DateTime end)
        {
            string url = string.Format("{0}/employeecost/{1}/{2}",
               ResourceUrl, start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"));

            return CreateGetRequest<List<EmployeeCostSummary>>(Guid.Empty, url);
        }

<<<<<<< HEAD
        public IList<UsageSummary> GetUsageSummary(DateTime start, DateTime end)
        {
            string url = string.Format("{0}/usage/{1}/{2}",
               ResourceUrl, start.ToString("YYYY-MM-dd"), end.ToString("YYYY-MM-dd"));

            return CreateGetRequest<List<UsageSummary>>(Guid.Empty, url);
=======
        public IList<EggProductionReport> GetEggProductionReport(DateTime start, DateTime end)
        {
            string url = string.Format("{0}/eggproduction/{1}/{2}",
               ResourceUrl, start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"));

            return CreateGetRequest<List<EggProductionReport>>(Guid.Empty, url);
>>>>>>> remotes/upstream/master
        }
    }
}
