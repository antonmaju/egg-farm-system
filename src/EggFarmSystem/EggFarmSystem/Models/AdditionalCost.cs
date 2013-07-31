using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models
{
    //electricity cost
    //phone cost
    //water cost
    //packing cost

    public class AdditionalCost :Entity
    {
        public string Name { get; set; }

        public long Value { get; set; }

        public override IList<ErrorInfo> Validate()
        {
            var errors = new List<ErrorInfo>();

            if(string.IsNullOrWhiteSpace(Name))
                errors.Add(new ErrorInfo("Name", "AdditionalCost_RequireName"));

            if(Value <= 0)
                errors.Add(new ErrorInfo("Value", "AdditionalCost_RequireValue"));

            return errors;
        }
    }
}
