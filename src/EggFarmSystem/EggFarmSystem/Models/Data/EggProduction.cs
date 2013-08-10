using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models.Data
{
    public class EggProduction : Entity
    {
        public DateTime Date { get; set; }
    }

    public class EggProductionDetail
    {
        public Guid ProductionId { get; set; }

        public Guid HouseId { get; set; }

        public int GoofEggCount { get; set; }

        public decimal RetailQuantity { get; set; }

        public decimal Fcr { get; set; }

        public int CrackedEggCount { get; set; }
    }
}
