using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models
{
    public class EggProduction :Entity
    {
        public EggProduction()
        {
            Details = new List<EggProductionDetail>();
        }

        public DateTime Date { get; set; }

        public List<EggProductionDetail> Details { get; set; } 
    }

    public class EggProductionDetail
    {
        public Guid HouseId { get; set; }

        public int GoodEggCount { get; set; }

        public decimal RetailQuantity { get; set; } //in kg

        public decimal Fcr { get; set; }

        public int CrackedEggCount { get; set; }
    }
}
