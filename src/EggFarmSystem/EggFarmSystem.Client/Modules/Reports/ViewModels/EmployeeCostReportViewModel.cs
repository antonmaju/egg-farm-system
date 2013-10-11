using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Core;
using MigraDoc.DocumentObjectModel;

namespace EggFarmSystem.Client.Modules.Reports.ViewModels
{
    public class EmployeeCostReportViewModel : ViewModelBase
    {
        private DateTime startDate;
        private DateTime endDate;
        private Document document;

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; OnPropertyChanged("StartDate"); }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; OnPropertyChanged("EndDate"); }
        }

        public Document Document
        {
            get { return document; }
            set
            {
                document = value;
                OnPropertyChanged("Document");
            }
        }
    }
}
