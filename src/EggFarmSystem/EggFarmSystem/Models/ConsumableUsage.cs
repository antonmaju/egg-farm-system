using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models
{
    public class ConsumableUsage : Entity
    {
        public ConsumableUsage()
        {
            Details = new List<ConsumableUsageDetail>();
        }

        public DateTime Date { get; set; }

        public long Total { get; set; }

        public List<ConsumableUsageDetail> Details { get; set; }

        public override IList<ErrorInfo> Validate()
        {
            var errors = new List<ErrorInfo>();

            if (Details == null || Details.Count == 0)
            {
                errors.Add(new ErrorInfo("Details", "Usage_RequireDetails"));
            }
            else
            {               
                if (Details.GroupBy(d => d.HouseId, d => d.ConsumableId).Any(g => g.Count() > 1))
                    errors.Add(new ErrorInfo("Details", "Usage_DuplicateDetails"));
                    //errors.Add(new ErrorInfo("Details", "Usage_DuplicateDe"));
                    

                for (var i = 0; i < Details.Count; i++)
                {
                    var detailErrors = Details[i].Validate();
                    if (detailErrors != null && detailErrors.Count > 0)
                    {
                        errors.AddRange(detailErrors.Select(detailError => new ErrorInfo("Details[" + i + "]." + detailError.PropertyName, detailError.Message)));
                    }
                }
            }

            return errors;
        }
    }

    public class ConsumableUsageDetail : IValidatable
    {
        public Guid HouseId { get; set; }

        public Guid ConsumableId { get; set; }

        public long Count { get; set; }

        public long UnitPrice { get; set; }

        public long SubTotal { get; set; }

        public IList<ErrorInfo> Validate()
        {
            var errors = new List<ErrorInfo>();

            if (HouseId == Guid.Empty)
                errors.Add(new ErrorInfo("HouseId", "Usage_RequireHouseId"));

            if (Count <= 0)
            {
                errors.Add(new ErrorInfo("Count", "Usage_InvalidCount"));
            }
            if (UnitPrice <= 0)
                errors.Add(new ErrorInfo("UnitPrice", "Usage_InvalidUnitPrice"));

            if(SubTotal <= 0)
                errors.Add(new ErrorInfo("SubTotal", "Usage_SubTotal"));

            return errors;
        }
    }
}
