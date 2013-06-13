using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models
{
    public class Employee :Entity
    {
        public string Name { get; set; }

        public long Salary { get; set; }
    }
}
