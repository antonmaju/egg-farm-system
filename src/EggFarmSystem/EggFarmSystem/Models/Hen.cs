using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models
{
    public class Hen : Entity
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public int Count { get; set; }

        public bool Active { get; set; }

        public long Cost { get; set; }

        public Guid HouseId { get; set; }
    }
}
