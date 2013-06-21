using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models.Data
{
    public class ConsumableUsage : Entity
    {
        public DateTime Date { get; set; }

        public long Total { get; set; }
    }

    public class ConsumableUsageDetail
    {
        public Guid UsageId { get; set; }

        public Guid HouseId { get; set; }

        public Guid ConsumableId { get; set; }

        public int Count { get; set; }

        public long UnitPrice { get; set; }

        public long SubTotal { get; set; }
    }
}
