using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.Reports.ViewModels
{
    public class ReportInfo
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Type ViewType { get; set; }
    }
}
