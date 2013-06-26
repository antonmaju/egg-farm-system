using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Models
{
    public interface IPagingInfo
    {
        int PageSize { get; set; }

        int PageIndex { get; set; }

        int TotalPage { get; set; }
    }
}
