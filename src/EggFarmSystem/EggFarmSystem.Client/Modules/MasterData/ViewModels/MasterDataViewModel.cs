using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Core;

namespace EggFarmSystem.Client.Modules.MasterData.ViewModels
{
    public class MasterDataViewModel : ViewModelBase
    {
        private bool _isEmployeeInput;
        private bool _isHenInput;
        private bool _isHouseInput;

        public MasterDataViewModel()
        {
            
        }

        public bool IsEmployeeInput
        {
            get { return _isEmployeeInput; }
            set {
                _isEmployeeInput = value;
                if(value) ResetSelection("Employee");
                OnPropertyChanged("IsEmployeeInput");
            }
        }

        public bool IsHenInput
        {
            get { return _isHenInput; }
            set {
                _isHenInput = value;
                if (value) ResetSelection("Hen");
                OnPropertyChanged("IsHenInput");
            }
        }

        public bool IsHouseInput
        {
            get { return _isHouseInput; }
            set 
            {
                _isHouseInput = value;
                if (value) ResetSelection("House");
                OnPropertyChanged("IsHouseInput");
            }
        }

        void ResetSelection(string mode)
        {
            switch (mode)
            {
                case "Hen":
                    IsEmployeeInput = false;
                    IsHouseInput = false;
                    break;

                case "House":
                    IsHenInput = false;
                    IsEmployeeInput = false;
                    break;

                case "Employee":
                    IsHenInput = false;
                    IsHouseInput = false;
                    break;
            }
        }

    }
}
