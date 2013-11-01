using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models.Reporting
{
    /// <summary>
    /// DTO for employee cost summary report
    /// </summary>
    public class UsageSummary
    {
        /// <summary>
        /// Gets or sets the consumable usage id.
        /// </summary>
        /// <value>consumable usage id.</value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets consumable usage name.
        /// </summary>
        /// <value>The consumable usage name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets total usage bought.
        /// </summary>
        /// <value>Total bought.</value>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets UnitPrice when bought.
        /// </summary>
        /// <value>Unit Price when bought.</value>
        public long UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets Usage Date.
        /// </summary>
        /// <value>Date when usage bought.</value>
        public DateTime UsageDate { get; set; }

        /// <summary>
        /// Gets or sets the subtotal of consumable usage that been bought.
        /// </summary>
        /// <value>The subtotal .</value>
        public long SubTotal { get; set; }
        
        /// <summary>
        /// Gets or sets hen house name.
        /// </summary>
        /// <value>HenHouseName</value>
        public string HenHouseName { get; set; } 
    }
}
