using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Models.Reporting;
using EggFarmSystem.Services;

namespace EggFarmSystem.Service.Controllers
{
    public class EmployeeCostSummaryController : ApiControllerBase
    {
        private readonly IReportingService service;

        public EmployeeCostSummaryController(IReportingService service)
        {
            this.service = service;
        }

        public IList<EmployeeCostSummary> Get(DateTime start, DateTime end)
        {
            return service.GetEmployeeCostSummary(start, end);
        }

    }

    public class EggProductionReportController : ApiControllerBase
    {
        private readonly IReportingService service;

        public EggProductionReportController(IReportingService service)
        {
            this.service = service;
        }

        public IList<EggProductionReport> Get(DateTime start, DateTime end)
        {
            return service.GetEggProductionReport(start, end);
        }
    }
}
