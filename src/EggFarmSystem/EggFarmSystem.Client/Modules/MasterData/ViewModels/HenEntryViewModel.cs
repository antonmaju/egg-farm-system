using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using AutoMapper;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.EggProduction.ViewModels;
using EggFarmSystem.Client.Modules.MasterData.Commands;
using EggFarmSystem.Client.Modules.MasterData.Views;
using EggFarmSystem.Models;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.MasterData.ViewModels
{
    public class HenEntryViewModel : ViewModelBase
    {
        private readonly IHenService henService;
        private readonly IHenHouseService houseService;
        private readonly IMessageBroker messageBroker;


        private ObservableCollection<HenHouse> henHouses;

        public HenEntryViewModel(IMessageBroker messageBroker, IHenService henService, IHenHouseService houseService,
            SaveHenCommand saveCommand, CancelCommand cancelCommand)
        {
            this.henService = henService;
            this.messageBroker = messageBroker;
            this.houseService = houseService;

            ActualSaveCommand = saveCommand;
            CancelCommand = cancelCommand;
            
            PropertiesToValidate = new List<string>
                {
                    "Name",
                    "Type",
                    "HouseId"
                };

            OnRefreshHouseList(null);

            InitializeCommands();
            SubscribeMessages();
        }

        #region commands

        private SaveHenCommand ActualSaveCommand { get; set; }

        public DelegateCommand SaveCommand { get; private set; }

        public CancelCommand CancelCommand { get; private set; }

        public IList<CommandBase> NavigationCommands { get; private set; }

        private void InitializeCommands()
        {
            CancelCommand.Action = broker => messageBroker.Publish(CommonMessages.ChangeMasterDataView, MasterDataTypes.Hen);

            SaveCommand = new DelegateCommand(Save, CanSave){Text = () => LanguageData.General_Save};

            NavigationCommands = new List<CommandBase>()
                {
                    SaveCommand, CancelCommand
                };
        }

        void Save(object param)
        {
            var hen = Mapper.Map<HenEntryViewModel, Hen>(this);
            ActualSaveCommand.Execute(hen);
        }

        bool CanSave(object param)
        {
            return IsValid();
        }

        #endregion

        public ObservableCollection<HenHouse> HenHouses
        {
            get { return henHouses; }
            private set 
            { 
                henHouses = value;
                OnPropertyChanged("HenHouses");
            }
        }  
      
        #region text
        
        public string NameText
        {
            get { return LanguageData.Hen_NameField; }
        }

        public string TypeText
        {
            get { return LanguageData.Hen_TypeField; }
        }

        public string CountText
        {
            get { return LanguageData.Hen_CountField; }
        }

        public string CostText
        {
            get { return LanguageData.Hen_CostField; }
        }

        public string ActiveText
        {
            get { return LanguageData.Hen_ActiveField; }
        }

        public string HouseText
        {
            get { return LanguageData.Hen_HouseField; }
        }

        #endregion

        #region properties

        private Guid id;
        private string name;
        private string type;
        private int count;
        private bool active;
        private long cost;
        private Guid houseId;

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

        public string Type
        {
            get { return type; }
            set 
            { 
                type = value;
                OnPropertyChanged("Type"); 
            }
        }

        public int Count
        {
            get { return count; }
            set 
            { 
                count = value;
                OnPropertyChanged("Count");
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

        public long Cost
        {
            get { return cost; }
            set 
            { 
                cost = value;
                OnPropertyChanged("Cost"); 
            }
        }

        public Guid HouseId
        {
            get { return houseId; }
            set 
            { 
                houseId = value;
                OnPropertyChanged("HouseId"); 
            }
        }

        #endregion

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
                                result = LanguageData.Hen_RequireName;
                        break;
                    case "Type":
                        if (string.IsNullOrWhiteSpace(Type))
                            result = LanguageData.Hen_RequireType;
                        break;

                    case "HouseId":
                        if (HouseId == Guid.Empty)
                            result = LanguageData.Hen_RequireHouse;
                        break;
                }
                return result;
            }
        }

        #endregion

        void SubscribeMessages()
        {
            messageBroker.Subscribe(CommonMessages.NewHenEntry, OnNewHen);
            messageBroker.Subscribe(CommonMessages.LoadHen, OnEditHen);
            messageBroker.Subscribe(CommonMessages.HenSavingFailed, OnHenSavingFailed);
            messageBroker.Subscribe(CommonMessages.SaveHouseSuccess, OnRefreshHouseList);
            messageBroker.Subscribe(CommonMessages.DeleteHouseSuccess, OnRefreshHouseList);
        }

        void OnRefreshHouseList(object param)
        {
            HenHouses = new ObservableCollection<HenHouse>(houseService.GetAll());
        }

        void OnHenSavingFailed(object param)
        {
            MessageBox.Show(param.ToString());
        }

        void OnEditHen(object param)
        {
            var loadedHen = henService.Get((Guid) param);
            ///TODO: will be done with automapper
            Id = loadedHen.Id;
            Name = loadedHen.Name;
            Type = loadedHen.Type;
            Count = loadedHen.Count;
            Active = loadedHen.Active;
            Cost = loadedHen.Cost;
            HouseId = loadedHen.HouseId;
        }

        void OnNewHen(object param)
        {
           Id = Guid.Empty;
           Name = string.Empty;
           Type = string.Empty;
           Count = 0;
           Active = true;
           Cost = 0;
           HouseId = Guid.Empty;
        }

        void UnsubscribeMessages()
        {
            messageBroker.Unsubscribe(CommonMessages.NewHenEntry, OnNewHen);
            messageBroker.Unsubscribe(CommonMessages.LoadHen, OnEditHen);
            messageBroker.Unsubscribe(CommonMessages.HenSavingFailed, OnHenSavingFailed);
            messageBroker.Unsubscribe(CommonMessages.SaveHouseSuccess, OnRefreshHouseList);
            messageBroker.Unsubscribe(CommonMessages.DeleteHouseSuccess, OnRefreshHouseList);
        }

        public override void Dispose()
        {
            UnsubscribeMessages();
            base.Dispose();
        }



    }
}
