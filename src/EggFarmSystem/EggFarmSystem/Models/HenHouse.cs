using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EggFarmSystem.Models
{
    public class HenHouse : Entity
    {
        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        public long PurchaseCost { get; set; }
        
        public int YearUsage { get; set; }

        public long Depreciation { get; set; }

        public bool Active { get; set; }
    }
}
