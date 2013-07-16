using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models.Data
{
    public class EmployeeCost : Entity
    {
        public DateTime Date { get; set; }

        public long Total { get; set; }
    }

    public class EmployeeCostDetail
    {
        public Guid CostId { get; set; }

        public Guid EmployeeId { get; set; }

        public bool Present { get; set; }

        public long Salary { get; set; }

        public string Description { get; set; }
    }
}
