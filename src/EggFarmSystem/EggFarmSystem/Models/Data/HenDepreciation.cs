using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models.Data
{
    public class HenDepreciation : Entity
    {
        public DateTime Date { get; set; }
    }

    public class HenDepreciationDetail
    {
        public Guid DepreciationId { get; set; }

        public Guid HouseId { get; set; }

        public decimal InitialPrice { get; set; }

        public decimal SellingPrice { get; set; }

        public decimal Profit { get; set; }

        public decimal Depreciation { get; set; }
    }
}
