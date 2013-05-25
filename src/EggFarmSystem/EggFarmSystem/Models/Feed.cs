using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models
{
    public class Feed : Entity
    {
        public string Name { get; set; }

        public int Stock { get; set; }

        public bool Active { get; set; }
    }
}
