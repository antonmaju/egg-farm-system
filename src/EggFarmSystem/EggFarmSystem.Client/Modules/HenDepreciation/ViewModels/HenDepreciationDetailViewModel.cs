using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.HenDepreciation.ViewModels
{
    public class HenDepreciationDetailViewModel :ViewModelBase
    {
        private Guid houseId;
        private decimal initialPrice;
        private decimal sellingPrice;
        private decimal profit;
        private decimal depreciation;

        public Guid HouseId
        {
            get { return houseId; }
            set { houseId = value; OnPropertyChanged("HouseId"); }
        }

        public decimal InitialPrice
        {
            get { return initialPrice; }
            set { initialPrice = value; OnPropertyChanged("InitialPrice"); UpdateProfit();  }
        }

        public decimal SellingPrice
        {
            get { return sellingPrice; }
            set { sellingPrice = value; OnPropertyChanged("SellingPrice"); UpdateProfit(); }
        }

        public decimal Profit
        {
            get { return profit; }
            set { profit = value; OnPropertyChanged("Profit"); }
        }

        private void UpdateProfit()
        {
            profit = SellingPrice - InitialPrice;
            OnPropertyChanged("Profit");
        }

        public override string this[string columnName]
        {
            get
            {
                string result = null;

                switch (columnName)
                {
                    case "HouseId":
                        if (HouseId == Guid.Empty)
                            result = LanguageData.HenDepreciationDetail_RequireHouseId;
                        break;
                    
                    case "InitialPrice":
                        if (InitialPrice < 0)
                            result = LanguageData.HenDepreciationDetail_InvalidInitialPrice;
                        break;

                    case "SellingPrice":
                        if (SellingPrice < 0)
                            result = LanguageData.HenDepreciationDetail_InvalidSellingPrice;
                        break;
                }

                return result;
            }
        }

        public static readonly string[] PropertiesToValidate =
            {
                "HouseId"
            };

        public override string Error
        {
            get 
            { 
                string error = null;

                foreach (var prop in PropertiesToValidate)
                {
                    error = this[prop];
                    
                    if(error != null)
                        break;
                }

                return error;
            }
        }
    }
}
