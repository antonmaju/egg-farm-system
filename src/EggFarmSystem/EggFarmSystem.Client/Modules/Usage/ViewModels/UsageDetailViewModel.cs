using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Core;

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
            set { consumableId = value; OnPropertyChanged("HouseId"); }
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
    }
}
