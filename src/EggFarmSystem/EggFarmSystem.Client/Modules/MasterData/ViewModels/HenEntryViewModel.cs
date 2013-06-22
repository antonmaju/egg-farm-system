using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
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
        private Hen hen;

        private ObservableCollection<HenHouse> henHouses;

        public HenEntryViewModel(IMessageBroker messageBroker, IHenService henService, IHenHouseService houseService,
            SaveHenCommand saveCommand, CancelCommand cancelCommand)
        {
            this.henService = henService;
            this.messageBroker = messageBroker;
            this.houseService = houseService;

            NavigationCommands = new List<CommandBase>()
                {
                    saveCommand, cancelCommand
                };

            hen = new Hen();
            SaveCommand = saveCommand;
            saveCommand.Hen = hen;

            cancelCommand.Action = broker => messageBroker.Publish(CommonMessages.ChangeMasterDataView, MasterDataTypes.Hen);

            OnRefreshHouseList(null);

            SubscribeMessages();
        }

        public SaveHenCommand SaveCommand { get; private set; }

        public ObservableCollection<HenHouse> HenHouses
        {
            get { return henHouses; }
            private set 
            { 
                henHouses = value;
                OnPropertyChanged("HenHouses");
            }
        } 

        public IList<CommandBase> NavigationCommands { get; private set; }
      
        #region text
        
        public string NameText
        {
            get { return LanguageData.Hen_NameField.ToUpper(); }
        }

        public string TypeText
        {
            get { return LanguageData.Hen_TypeField.ToUpper(); }
        }

        public string CountText
        {
            get { return LanguageData.Hen_CountField.ToUpper(); }
        }

        public string CostText
        {
            get { return LanguageData.Hen_CostField.ToUpper(); }
        }

        public string ActiveText
        {
            get { return LanguageData.Hen_ActiveField.ToUpper(); }
        }

        public string HouseText
        {
            get { return LanguageData.Hen_HouseField.ToUpper(); }
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
                hen.Id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Name
        {
            get { return name; }
            set { name = value;
                hen.Name = name;
                OnPropertyChanged("Name");
            }
        }

        public string Type
        {
            get { return type; }
            set { type = value;
                hen.Type = type;
                OnPropertyChanged("Type"); }
        }

        public int Count
        {
            get { return count; }
            set { 
                count = value;
                hen.Count = count;
                OnPropertyChanged("Count");
            }
        }

        public bool Active
        {
            get { return active; }
            set { 
                active = value;
                hen.Active = active;
                OnPropertyChanged("Active"); }
        }

        public long Cost
        {
            get { return cost; }
            set { cost = value;
                hen.Cost = value;
                OnPropertyChanged("Cost"); }
        }

        public Guid HouseId
        {
            get { return houseId; }
            set { houseId = value;
                hen.HouseId = value; OnPropertyChanged("HouseId"); }
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
                            result = LanguageData.Hen_RequireName;
                        break;
                    case "Type":
                        if (string.IsNullOrWhiteSpace(Type))
                            result = LanguageData.Hen_RequireType;
                        break;
                }

                return result;
            }
        }

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
            messageBroker.Subscribe(CommonMessages.LoadHen, OnEditHen);
            messageBroker.Unsubscribe(CommonMessages.HenSavingFailed, OnHenSavingFailed);
            messageBroker.Unsubscribe(CommonMessages.SaveHouseSuccess, OnRefreshHouseList);
            messageBroker.Unsubscribe(CommonMessages.DeleteHouseSuccess, OnRefreshHouseList);
        }

        public override void Dispose()
        {
            UnsubscribeMessages();
            SaveCommand.Hen = null;
            base.Dispose();
        }



    }
}
