using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models
{
    public class SearchInfo
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }

    public class ConsumableUsageSearchInfo : SearchInfo
    {
        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }
    }
}
