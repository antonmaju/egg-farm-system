using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models
{
    public class EggProduction :Entity
    {
        public EggProduction()
        {
            Details = new List<EggProductionDetail>();
        }

        public DateTime Date { get; set; }

        public List<EggProductionDetail> Details { get; set; }

        public override IList<ErrorInfo> Validate()
        {
            var errors = new List<ErrorInfo>();

            if (Date == DateTime.MaxValue || Date == DateTime.MinValue)
                errors.Add(new ErrorInfo("Date", "EggProduction_InvalidDate"));

            if (Details == null || Details.Count == 0)
            {
                errors.Add(new ErrorInfo("Details","EggProduction_RequireDetails"));
            }
            else
            {
                if (Details.GroupBy(d => d.HouseId).Any(g => g.Count() > 1))
                    errors.Add(new ErrorInfo("Details", "EggProduction_DuplicateHouse"));

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

    public class EggProductionDetail : IValidatable
    {
        public Guid HouseId { get; set; }

        public int GoodEggCount { get; set; }

        public decimal RetailQuantity { get; set; } //in kg

        public decimal Fcr { get; set; }

        public int CrackedEggCount { get; set; }

        public IList<ErrorInfo> Validate()
        {
            var errors = new List<ErrorInfo>();

            if(GoodEggCount < 0)
                errors.Add(new ErrorInfo("GoodEggCount", "EggProductionDetail_InvalidGoodEggCount"));

            if(RetailQuantity < 0)
                errors.Add(new ErrorInfo("RetailQuantity","EggProductionDetail_InvalidRetailQuantity"));

            if(Fcr < 0)
                errors.Add(new ErrorInfo("Fcr", "EggProductionDetail_InvalidFcr"));

            return errors;
        }
    }
}
