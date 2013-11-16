using System.Windows;
using AutoMapper;
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

        public HouseEntryViewModel(IMessageBroker messageBroker, IHenHouseService houseService,
            SaveHenHouseCommand saveCommand, CancelCommand cancelCommand)
        {
            this.houseService = houseService;
            this.messageBroker = messageBroker;

            ActualSaveCommand = saveCommand;
            SubscribeMessages();

            PropertiesToValidate = new List<string>
                {
                    "Name",
                    "PurchaseCost",
                    "YearUsage",
                    "ProductiveAge"
                };

            CancelCommand = cancelCommand;
            InitializeCommands();
        }

        #region commands

        public IList<CommandBase> NavigationCommands { get; private set; }

        private SaveHenHouseCommand ActualSaveCommand { get; set; }

        private CancelCommand CancelCommand { get; set; }

        public DelegateCommand SaveCommand { get; private set; }

        private void InitializeCommands()
        {
            CancelCommand.Action = broker => messageBroker.Publish(CommonMessages.ChangeMasterDataView, MasterDataTypes.HenHouse);
            SaveCommand = new DelegateCommand(Save, CanSave){Text = ()=> LanguageData.General_Save};
            NavigationCommands = new List<CommandBase> { SaveCommand, CancelCommand };
        }

        private void Save(object param)
        {
            var house = Mapper.Map<HouseEntryViewModel, HenHouse>(this);
            ActualSaveCommand.Execute(house);
        }

        private bool CanSave(object param)
        {
            return IsValid();
        }

        #endregion

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

        public string WeightField
        {
            get { return LanguageData.House_WeightField; }
        }


        #endregion

        #region properties

        private Guid id;
        private string name;
        private long purchaseCost;
        private int yearUsage;
        private decimal depreciation;
        private bool active;
        private int population;
        private int productiveAge;
        private decimal weight;

        public Guid Id
        {
            get { return id; }
            set 
            { 
                id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public long PurchaseCost
        {
            get { return purchaseCost; }
            set
            {
                purchaseCost = value;
                OnPropertyChanged("PurchaseCost");CalculateDepreciation();
            }
        }

        public int YearUsage
        {
            get { return yearUsage; }
            set 
            { 
                yearUsage = value;    
                OnPropertyChanged("YearUsage"); 
            }
        }

        public decimal Depreciation
        {
            get { return depreciation; }
            set
            {
                depreciation = value;
                OnPropertyChanged("Depreciation");
            }
        }

        public bool Active
        {
            get { return active; }
            set 
            { 
                active = value;
                OnPropertyChanged("Active"); 
            }
        }

        public int Population
        {
            get { return population; }
            set 
            { 
                population = value; 
                OnPropertyChanged("Population"); 
                CalculateDepreciation(); 
            }
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

        public decimal Weight
        {
            get { return weight; }
            set 
            { 
                weight = value;
                OnPropertyChanged("Weight");CalculateDepreciation();
            }
        }

        #endregion

        private void CalculateDepreciation()
        {
            if (Population == 0)
            {
                Depreciation = 0;
                return;
            }

            Depreciation = Weight*PurchaseCost/Population;
        }

        #region validation

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
                    case "ProductiveAge":
                        if (ProductiveAge <= 0)
                            result = LanguageData.House_RequireProductiveAge;
                        break;
                }

                return result;
            }
        }

        #endregion

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
            Weight = 0;
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
            YearUsage = loadedHouse.YearUsage;
            ProductiveAge = loadedHouse.ProductiveAge;
            Weight = loadedHouse.Weight;

            Population = houseService.GetPopulation(loadedHouse.Id);
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
            base.Dispose();
        }
    }
}
