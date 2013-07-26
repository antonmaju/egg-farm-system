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
            
            if (Details == null || Details.Count == 0)
            {
                errorList.Add(new ErrorInfo("Details", "EmployeeCost_RequireDetails"));
            }
            else
            {
                if (Details.GroupBy(d => d.EmployeeId).Any(g => g.Count() > 1))
                    errorList.Add(new ErrorInfo("Details", "EmployeeCost_DuplicateEmployee"));            

                for (var i = 0; i < Details.Count; i++)
                {
                    var detailErrors = Details[i].Validate();
                    if (detailErrors != null && detailErrors.Count > 0)
                    {
                        errorList.AddRange(detailErrors.Select(detailError => new ErrorInfo("Details[" + i + "]." + detailError.PropertyName, detailError.Message)));
                    }
                }
            }

            return errorList;
        }
    }

    public class EmployeeCostDetail : IValidatable
    {
        public Guid EmployeeId { get; set; }

        public bool Present { get; set; }

        public long Salary { get; set; }

        public string Description { get; set; }

        public IList<ErrorInfo> Validate()
        {
            var errors = new List<ErrorInfo>();

            if(EmployeeId == Guid.Empty)
                errors.Add(new ErrorInfo("EmployeeId","EmployeeCostDetail_RequireEmployee"));

            return errors;
        }
    }
}
