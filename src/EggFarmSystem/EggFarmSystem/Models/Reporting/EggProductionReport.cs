using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models.Reporting
{
    public class EggProductionReport
    {
        public EggProductionReport()
        {
            Details = new List<EggProductionReportDetail>();
        }

        public Guid  Id { get; set; }     
    
        public DateTime Date { get; set; }

        public List<EggProductionReportDetail> Details { get; set; }
    }

    public class EggProductionReportDetail
    {
        public HouseInfo House { get; set; }

        public int GoodEggCount { get; set; }

        public decimal RetailQuantity { get; set; }

        public decimal Fcr { get; set; }

        public int CrackedEggCount { get; set; }
    }
}
