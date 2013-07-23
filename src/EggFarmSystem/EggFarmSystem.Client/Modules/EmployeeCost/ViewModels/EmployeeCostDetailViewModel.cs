using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.EmployeeCost.ViewModels
{
    public class EmployeeCostDetailViewModel : ViewModelBase
    {
        private Guid employeeId;
        private bool present;
        private long salary;
        private string description;
        
        public Guid EmployeeId
        {
            get { return employeeId; }
            set { employeeId = value; OnPropertyChanged("EmployeeId"); }
        }

        public bool Present
        {
            get { return present; }
            set { present = value; OnPropertyChanged("Present"); }
        }

        public long Salary
        {
            get { return salary; }
            set { salary = value; OnPropertyChanged("Salary"); }
        }

        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged("Description"); }
        }

        public override string this[string columnName]
        {
            get
            {
                string result = null;

                switch (columnName)
                {
                    case "EmployeeId":
                        if (EmployeeId == Guid.Empty)
                            result = LanguageData.EmployeeCostDetail_RequireEmployee;
                        break;
                }

                return result;
            }
        }

        public static readonly string[] PropertiesToValidate =
            {
                "EmployeeId"
            };

        public override string Error
        {
            get
            {
                string error = null;

                foreach (var prop in PropertiesToValidate)
                {
                    error = this[prop];
                    if (error != null)
                        break;
                }

                return error;
            }
        }
    }
}
