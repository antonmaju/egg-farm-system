using System;
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

        public override IList<ErrorInfo> Validate()
        {
            var errors = new List<ErrorInfo>();

            if(string.IsNullOrWhiteSpace(Name))
                errors.Add(new ErrorInfo("Name", "Consumable_RequireName"));

            if(UnitPrice <= 0)
                errors.Add(new ErrorInfo("UnitPrice", "Consumable_RequireUnitPrice"));

            return errors;
        }
    }


    public enum ConsumableType
    {
        Feed,
        Ovk
    }
}
