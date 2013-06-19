using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EggFarmSystem.Models
{
    public class HenHouse : Entity
    {
        [StringLength(30)]
        public string Name { get; set; }

        public long PurchaseCost { get; set; }
        
        public int YearUsage { get; set; }

        public long Depreciation { get; set; }

        public bool Active { get; set; }

        public override IList<ErrorInfo> Validate()
        {
            var errors = new List<ErrorInfo>();

            if (string.IsNullOrEmpty(Name))
                errors.Add(new ErrorInfo("Name","House_RequireName"));
         
            if(PurchaseCost <= 0)
                errors.Add(new ErrorInfo("PurchaseCost", "House_RequirePurchaseCost"));

            if(YearUsage <= 0)
                errors.Add(new ErrorInfo("YearUsage", "House_RequireYearUsage"));

            if(Depreciation <= 0)
                errors.Add(new ErrorInfo("Depreciation", "House_RequireDepreciation"));

            return errors;
        }
    }
}
