using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models
{
    public class EmployeeCost : Entity
    {
        public EmployeeCost()
        {
            Details = new List<EmployeeCostDetail>();
        }

        public DateTime Date { get; set; }

        public long Total { get; set; }

        public List<EmployeeCostDetail> Details { get; set; }

        public override IList<ErrorInfo> Validate()
        {
            var errorList = new List<ErrorInfo>();

            if(Date == DateTime.MaxValue || Date == DateTime.MinValue)
                errorList.Add(new ErrorInfo("Date","EmployeeCost_InvalidDate"));
            
            return errorList;
        }
    }

    public class EmployeeCostDetail
    {
        public Guid EmployeeId { get; set; }

        public bool Present { get; set; }

        public long Salary { get; set; }

        public string Description { get; set; }
    }
}
