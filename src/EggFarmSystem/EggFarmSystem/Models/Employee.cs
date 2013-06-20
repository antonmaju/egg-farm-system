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

        public override IList<ErrorInfo> Validate()
        {
            var errors = new List<ErrorInfo>();

            if(string.IsNullOrWhiteSpace(Name))
                errors.Add(new ErrorInfo("Name", "Employee_RequireName"));

            if(Salary <= 0)
                errors.Add(new ErrorInfo("Salary", "Employee_RequireSalary"));

            return errors;
        }
    }
}
