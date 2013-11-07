using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models.Reporting
{
    /// <summary>
    /// DTO for usage summary report
    /// </summary>
    public class UsageSummary
    {

        public UsageSummary()
        {
            Details = new List<UsageSummaryDetail>();
        }

        

        public List<UsageSummaryDetail> Details { get; set; }
        
        /// <summary>
        /// Gets or sets the consumable usage id.
        /// </summary>
        /// <value>consumable usage id.</value>
        public Guid Id { get; set; }


        /// <summary>
        /// Gets or sets Usage Date.
        /// </summary>
        /// <value>Date when usage bought.</value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the subtotal of consumable usage that been bought.
        /// </summary>
        /// <value>The subtotal .</value>
        public long Total { get; set; }

    }
    public class UsageSummaryDetail
    {
        public HouseInfo House { get; set; }

        public ConsumableUsageInfo Consumable { get; set; }

        public int Count { get; set; }

        public long UnitPrice { get; set; }

        public long SubTotal { get; set; }

    }
}
