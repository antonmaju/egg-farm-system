﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Resources;

namespace EggFarmSystem.Client.Modules.EggProduction.ViewModels
{
    public class EggProductionDetailViewModel : ViewModelBase
    {
        private Guid houseId;
        private int goodEggCount;
        private decimal retailQuantity;
        private decimal fcr;
        private int crackedEggCount;
        private decimal feedTotal;

        public Guid HouseId
        {
            get { return houseId; }
            set
            {
                houseId = value;
                OnPropertyChanged("HouseId");
            }
        }

        public int GoodEggCount
        {
            get { return goodEggCount; }
            set
            {
                goodEggCount = value;
                OnPropertyChanged("GoodEggCount");
            }
        }

        public decimal RetailQuantity
        {
            get { return retailQuantity; }
            set
            {
                retailQuantity = value;
                OnPropertyChanged("RetailQuantity");
                CalculateFcr();
            }
        }

        public decimal Fcr
        {
            get { return fcr; }
            set 
            {
                fcr = value;
                OnPropertyChanged("Fcr");
            }
        }

        public int CrackedEggCount
        {
            get { return crackedEggCount; }
            set
            {
                crackedEggCount = value;
                OnPropertyChanged("CrackedEggCount");
            }
        }

        public decimal FeedTotal
        {
            get { return feedTotal; }
            set
            {
                feedTotal = value;
                OnPropertyChanged("FeedTotal");
                CalculateFcr();
            }
        }

        public override string this[string columnName]
        {
            get 
            {                 
                string result = null;

                switch (columnName)
                {
                    case "GoodEggCount":
                        if (GoodEggCount < 0)
                            result = LanguageData.EggProductionDetail_InvalidGoodEggCount;
                        break;
                    case "RetailQuantity":
                        if (RetailQuantity < 0)
                            result = LanguageData.EggProductionDetail_InvalidRetailQuantity;
                        break;
                    case "Fcr":
                        if (Fcr < 0)
                            result = LanguageData.EggProductionDetail_InvalidFcr;
                        break;
                }

                return result;
            }
        }

        public static readonly string[] PropertiesToValidate =
            {
                "GoodEggCount", "RetailQuantity","Fcr"
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

        private void CalculateFcr()
        {
            if (retailQuantity == 0)
            {
                Fcr = 0;
                return;
            }

            Fcr = FeedTotal/retailQuantity;
        }
    }
}
