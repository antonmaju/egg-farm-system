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

        public override IList<ErrorInfo> Validate()
        {
            var errors = new List<ErrorInfo>();

            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrEmpty(Name)) 
                errors.Add(new ErrorInfo("Name", "Hen_RequireName"));

            if(string.IsNullOrWhiteSpace(Type))
                errors.Add(new ErrorInfo("Type","Hen_RequireType"));

            if(Cost==0)
                errors.Add(new ErrorInfo("Cost", "Hen_RequireCost"));

            return errors;
        }
    }
}
