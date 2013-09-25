using System.Windows;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Commands;
using EggFarmSystem.Client.Modules.MasterData.Views;
using EggFarmSystem.Models;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.ViewModels
{
    public class HouseEntryViewModel : ViewModelBase
    {
        private readonly IHenHouseService houseService;
        private readonly IMessageBroker messageBroker;
        private readonly HenHouse house;

        public HouseEntryViewModel(IMessageBroker messageBroker, IHenHouseService houseService,
            SaveHenHouseCommand saveCommand, CancelCommand cancelCommand)
        {
            this.houseService = houseService;
            this.messageBroker = messageBroker;
            this.house = new HenHouse();

            SaveCommand = saveCommand;
            SaveCommand.HenHouse = house;
            SubscribeMessages();

            cancelCommand.Action = broker => messageBroker.Publish(CommonMessages.ChangeMasterDataView, MasterDataTypes.HenHouse);

           // messageBroker.Publish(CommonMessages.ChangeMasterDataView, MasterDataTypes.Consumable);

            NavigationCommands = new List<CommandBase>() {SaveCommand, cancelCommand};
        }

        public IList<CommandBase> NavigationCommands { get; private set; }

        public SaveHenHouseCommand SaveCommand { get; private set; }

        #region text

        public string NameField
        {
            get { return LanguageData.House_NameField; }
        }

        public string PurchaseCostField
        {
            get { return LanguageData.House_PurchaseCostField; }
        }

        public string YearUsageField
        {
            get { return LanguageData.House_YearUsageField; }
        }

        public string DepreciationField
        {
            get { return LanguageData.House_DepreciationField; }
        }

        public string ActiveField
        {
            get { return LanguageData.House_ActiveField; }
        }

        public string PopulationField
        {
            get { return LanguageData.House_PopulationField; }
        }

        public string ProductiveAgeField
        {
            get { return LanguageData.House_ProductiveAgeField; }
        }

       
        #endregion

        #region properties

        private Guid id;
        private string name;
        private long purchaseCost;
        private int yearUsage;
        private long depreciation;
        private bool active;
        private int population;
        private int productiveAge;

        public Guid Id
        {
            get { return id; }
            set { id = value;
                house.Id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                house.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public long PurchaseCost
        {
            get { return purchaseCost; }
            set
            {
                purchaseCost = value;
                house.PurchaseCost = value;
                OnPropertyChanged("PurchaseCost");
            }
        }

        public int YearUsage
        {
            get { return yearUsage; }
            set { 
                yearUsage = value;
                house.YearUsage = value;
                OnPropertyChanged("YearUsage"); }
        }

        public long Depreciation
        {
            get { return depreciation; }
            set
            {
                depreciation = value;
                house.Depreciation = value;
                OnPropertyChanged("Depreciation");
            }
        }

        public bool Active
        {
            get { return active; }
            set { 
                active = value;
                house.Active = active;
                OnPropertyChanged("Active"); 
            }
        }

        public int Population
        {
            get { return population; }
            set { population = value; 
                OnPropertyChanged("Population"); }
        }

        public int ProductiveAge
        {
            get { return productiveAge; }
            set 
            { 
                productiveAge = value;
                OnPropertyChanged("ProductiveAge");
            }
        }

        #endregion

        public override string this[string columnName]
        {
            get
            {
                string result = null;

                switch (columnName)
                {
                    case "Name":
                        if (string.IsNullOrWhiteSpace(Name))
                            result = LanguageData.House_RequireName;
                        break;
                    case "PurchaseCost":
                        if (PurchaseCost < 0)
                            result = LanguageData.House_RequirePurchaseCost;
                        break;
                    case "YearUsage":
                        if (YearUsage <= 0)
                            result = LanguageData.House_RequireYearUsage;
                        break;
                    case "Depreciation":
                        if (Depreciation <= 0)
                            result = LanguageData.House_RequireDepreciation;
                        break;
                    case "ProductiveAge":
                        if (ProductiveAge <= 0)
                            result = LanguageData.House_RequireProductiveAge;
                        break;
                }

                return result;
            }
        }

        void SubscribeMessages()
        {
            messageBroker.Subscribe(CommonMessages.NewHouseEntry, OnNewHouse);
            messageBroker.Subscribe(CommonMessages.LoadHouse, OnLoadHouse);
            messageBroker.Subscribe(CommonMessages.HenSavingFailed, OnHouseSavingFailed);
        }

        void OnNewHouse(object param)
        {
            Id = Guid.Empty;
            Name = string.Empty;
            PurchaseCost = 0;
            Depreciation = 0;
            Active = true;
            Population = 0;
            YearUsage = 0;
            ProductiveAge = 0;
        }

        void OnLoadHouse(object param)
        {
            var loadedHouse = houseService.Get((Guid) param);

            Id = loadedHouse.Id;
            Name = loadedHouse.Name;
            PurchaseCost = loadedHouse.PurchaseCost;
            Depreciation = loadedHouse.Depreciation;
            Active = loadedHouse.Active;
            Population = 0;
            YearUsage = loadedHouse.YearUsage;
            ProductiveAge = loadedHouse.ProductiveAge;
        }

        void OnHouseSavingFailed(object param)
        {
            MessageBox.Show(param.ToString());
        }

        void UnsubscribeMessages()
        {
            messageBroker.Unsubscribe(CommonMessages.NewHouseEntry, OnNewHouse);
            messageBroker.Unsubscribe(CommonMessages.LoadHouse, OnLoadHouse);
            messageBroker.Unsubscribe(CommonMessages.HenSavingFailed, OnHouseSavingFailed);
        }

        public override void Dispose()
        {
            UnsubscribeMessages();
            SaveCommand.HenHouse = null;
            base.Dispose();
        }
    }
}
