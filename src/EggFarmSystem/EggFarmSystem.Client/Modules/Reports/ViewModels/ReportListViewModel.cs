using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.Reports.Views;
using EggFarmSystem.Resources;
using MigraDoc.DocumentObjectModel;

namespace EggFarmSystem.Client.Modules.Reports.ViewModels
{
    public class ReportListViewModel : ViewModelBase
    {
        public ReportListViewModel()
        {
            ReportList = new List<ReportInfo>()
                {
                    new ReportInfo
                        {
                            Name = LanguageData.EmployeeCostReport_Title,
                            Description = LanguageData.EmployeeCostReport_Description,
                            ViewType = typeof(IEmployeeCostReportView)
                        },
                    new ReportInfo
                        {
                            Name = LanguageData.UsageReport_Title,
                            Description = LanguageData.UsageReport_Description,
                            ViewType = typeof(IUsageReportView)
                        },
                     new ReportInfo
                         {
                             Name = LanguageData.EggProductionReport_Title,
                             Description = LanguageData.EggProductionReport_Description
                         }
                };
        }

        public IList<ReportInfo> ReportList { get; private set; }

        #region reports

        public string NameText { get { return LanguageData.Reports_NameField; } }

        public string DescriptionText { get { return LanguageData.Reports_DescriptionField; } }

        #endregion
    }
}
