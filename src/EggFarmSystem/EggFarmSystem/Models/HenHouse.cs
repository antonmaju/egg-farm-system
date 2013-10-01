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

        public bool Active { get; set; }

        public int ProductiveAge { get; set; }

        public decimal Weight { get; set; }

        public decimal Depreciation { get; set; }

        public override IList<ErrorInfo> Validate()
        {
            var errors = new List<ErrorInfo>();

            if (string.IsNullOrEmpty(Name))
                errors.Add(new ErrorInfo("Name","House_RequireName"));
         
            if(PurchaseCost <= 0)
                errors.Add(new ErrorInfo("PurchaseCost", "House_RequirePurchaseCost"));

            if(YearUsage <= 0)
                errors.Add(new ErrorInfo("YearUsage", "House_RequireYearUsage"));

            if(ProductiveAge <= 0)
                errors.Add(new ErrorInfo("ProductiveAge", "House_RequireProductiveAge"));

            if(Weight <= 0)
                errors.Add(new ErrorInfo("Weight","House_RequireWeight"));

            return errors;
        }
    }
}
