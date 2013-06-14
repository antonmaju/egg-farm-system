﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models
{
    public class Consumable : Entity
    {
        public string Name { get; set; }

        public byte Type { get; set; }

        public string Unit { get; set; }

        public long UnitPrice { get; set; }

        public bool Active { get; set; }
    }


    public enum ConsumableType
    {
        Feed,
        Ovk
    }
}
