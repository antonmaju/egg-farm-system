using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models
{
    public class HenDepreciation : Entity
    {
        public HenDepreciation()
        {
            Details = new List<HenDepreciationDetail>();
        }

        public DateTime Date { get; set; }

        public List<HenDepreciationDetail> Details { get; set; }

        public override IList<ErrorInfo> Validate()
        {
            var errors = new List<ErrorInfo>();

            if (Details == null || Details.Count == 0)
            {
                errors.Add(new ErrorInfo("Details", "HenDepreciation_RequireDetails"));
            }
            else
            {
                if (Details.GroupBy(d => d.HouseId).Any(g => g.Count() > 1))
                    errors.Add(new ErrorInfo("Details", "HenDepreciation_DuplicateHouse"));

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

    public class HenDepreciationDetail : IValidatable
    {
        public Guid HouseId { get; set; }

        public decimal InitialPrice { get; set; }

        public decimal SellingPrice { get; set; }

        public decimal Profit
        {
            get { return SellingPrice - InitialPrice; }
        }

        public decimal Depreciation { get; set; }

        public IList<ErrorInfo> Validate()
        {
            var errors = new List<ErrorInfo>();

            if(HouseId == Guid.Empty)
                errors.Add(new ErrorInfo("HouseId", "HenDepreciationDetail_RequireHouseId"));

            if(InitialPrice < 0)
                errors.Add(new ErrorInfo("InitialPrice", "HenDepreciationDetail_InvalidInitialPrice"));

            if(SellingPrice < 0)
                errors.Add(new ErrorInfo("SellingPrice", "HenDepreciationDetail_InvalidSellingPrice"));

            return errors;
        }
    }
}
