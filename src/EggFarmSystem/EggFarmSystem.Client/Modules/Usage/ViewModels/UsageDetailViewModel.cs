using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.Usage.ViewModels
{
    public class UsageDetailViewModel : ViewModelBase
    {
        private Guid houseId;
        private Guid consumableId;
        private long count;
        private long unitPrice;
        private long subTotal;

        public Guid HouseId
        {
            get { return houseId; }
            set { houseId = value; OnPropertyChanged("HouseId"); }
        }

        public Guid ConsumableId
        {
            get { return consumableId; }
            set { consumableId = value; OnPropertyChanged("ConsumableId"); }
        }

        public long Count
        {
            get { return count; }
            set
            {
                count = value; 
                CalculateSubTotal();
                OnPropertyChanged("Count");
            }
        }

        public long UnitPrice
        {
            get { return unitPrice; }
            set
            {
                unitPrice = value; 
                CalculateSubTotal();
                OnPropertyChanged("UnitPrice");
            }
        }

        public long SubTotal
        {
            get { return subTotal; }
            set { subTotal = value; OnPropertyChanged("SubTotal"); }
        }

        public void CalculateSubTotal()
        {
            SubTotal = Count*UnitPrice;
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
                            result = LanguageData.UsageDetail_RequireHouse;
                        break;

                    case "ConsumableId":
                        if (ConsumableId == Guid.Empty)
                            result = LanguageData.UsageDetail_RequireConsumable;
                        break;

                    case "Count":
                        if (ConsumableId == Guid.Empty)
                            result = LanguageData.UsageDetail_RequireCount;
                        break;
                }
                return result;
            }
        }

        private static readonly string[] PropertiesToValidate =
            {
                "HouseId",
                "ConsumableId",
                "Count"
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
