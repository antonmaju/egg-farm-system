using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models
{
    public class ConsumableUsage : Entity
    {
        public ConsumableUsage()
        {
            Details = new List<ConsumableUsageDetail>();
        }

        public DateTime Date { get; set; }

        public long Total { get; set; }

        public List<ConsumableUsageDetail> Details { get; set; } 
    }

    public class ConsumableUsageDetail
    {
        public Guid HouseId { get; set; }

        public Guid ConsumableId { get; set; }

        public long Count { get; set; }

        public long UnitPrice { get; set; }

        public long SubTotal { get; set; }
    }
}
