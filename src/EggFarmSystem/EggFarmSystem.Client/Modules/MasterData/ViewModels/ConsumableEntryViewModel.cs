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
using System.Windows;

namespace EggFarmSystem.Client.Modules.MasterData.ViewModels
{
    public class ConsumableEntryViewModel : ViewModelBase
    {
        private readonly IMessageBroker messageBroker;
        private readonly IConsumableService consumableService;


        public ConsumableEntryViewModel(IMessageBroker messageBroker, IConsumableService consumableService, 
            SaveConsumableCommand saveCommand, CancelCommand cancelCommand)
        {
            this.consumableService = consumableService;
            this.messageBroker = messageBroker;

            ActualSaveCommand = saveCommand;
            CancelCommand = cancelCommand;
          
            ConsumableTypes = new List<Tuple<byte, string>>()
                {
                    Tuple.Create((byte)ConsumableType.Feed, LanguageData.ConsumableType_Feed),
                    Tuple.Create((byte)ConsumableType.Ovk, LanguageData.ConsumableType_Ovk)
                };

            PropertiesToValidate = new List<string>() { "Name", "UnitPrice" };
            
            InitializeCommands();
            SubscribeMessages();
        }

        
        public IList<Tuple<byte, string>> ConsumableTypes { get; private set; }

        #region commands

        public IList<CommandBase> NavigationCommands { get; private set; }

        public CancelCommand CancelCommand { get; private set; }

        private SaveConsumableCommand ActualSaveCommand { get; set; }

        public DelegateCommand SaveCommand { get; private set; }

        private void InitializeCommands()
        {
            CancelCommand.Action = broker => broker.Publish(CommonMessages.ChangeMasterDataView, MasterDataTypes.Consumable);

            SaveCommand = new DelegateCommand(Save, param => IsValid()) { Text=()=>LanguageData.General_Save };

            NavigationCommands = new List<CommandBase>() { SaveCommand,CancelCommand }; 
        }

        private void Save(object param)
        {
            var consumable = Mapper.Map<ConsumableEntryViewModel, Consumable>(this);
            ActualSaveCommand.Execute(consumable);
        }

        #endregion

        #region text

        public string NameText { get { return LanguageData.Consumable_NameField; } }

        public string TypeText { get { return LanguageData.Consumable_TypeField; } }

        public string UnitText { get { return LanguageData.Consumable_UnitField; } }

        public string UnitPriceText { get { return LanguageData.Consumable_UnitPriceField; } }

        public string ActiveText { get { return LanguageData.Consumable_ActiveField; } }

        #endregion

        #region properties

        private Guid id;
        private string name;
        private byte type;
        private string unit;
        private long unitPrice;
        private bool active;
        private bool isUnitReadOnly;

        public Guid Id
        {
            get { return id; }
            set 
            { 
                id = value;
                OnPropertyChanged("Id");
            }
        }

        public byte Type
        {
            get { return type; }
            set
            {
                type = value;
                AdjustUnit();
                OnPropertyChanged("Type");
               
            }
        }

        public string Unit
        {
            get { return unit; }
            set
            {
                unit = value;
                OnPropertyChanged("Unit");
            }
        }

        public long UnitPrice
        {
            get { return unitPrice; }
            set
            {
                unitPrice = value;
                OnPropertyChanged("UnitPrice");
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

        public string Name
        {
            get { return name; }
            set { name = value;
                OnPropertyChanged("Name");
            }
        }

        public bool IsUnitReadOnly
        {
            get { return isUnitReadOnly; }
            set 
            { 
                isUnitReadOnly = value;
                OnPropertyChanged("IsUnitReadOnly");
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
                            result = LanguageData.Consumable_RequireName;
                        break;

                    case "UnitPrice":
                        if (UnitPrice <= 0)
                            result = LanguageData.Consumable_RequireUnitPrice;    
                        break;
                }

                return result;
            }
        }

        void SubscribeMessages()
        {
            messageBroker.Subscribe(CommonMessages.NewConsumableEntry, OnNewConsumable);
            messageBroker.Subscribe(CommonMessages.LoadConsumable, OnLoadConsumable);
            messageBroker.Subscribe(CommonMessages.SaveConsumableFailed, OnSaveConsumableFailed);
        }

        void OnNewConsumable(object param)
        {
            Id = Guid.Empty;
            Name = string.Empty;
            Type = (byte) ConsumableType.Feed;
            Unit = string.Empty;
            UnitPrice = 0;
            Active = true;
            AdjustUnit();
        }

        private void OnLoadConsumable(object param)
        {
            var loadedConsumable = consumableService.Get((Guid) param);

            //TODO: consider using automapper
            Id = loadedConsumable.Id;
            Name = loadedConsumable.Name;
            Type = loadedConsumable.Type;
            Unit = loadedConsumable.Unit;
            UnitPrice = loadedConsumable.UnitPrice;
            Active = loadedConsumable.Active;
            
            AdjustUnit();
        }

        void OnSaveConsumableFailed(object param)
        {
            var error = param as Error;
            if (error == null) return;
            MessageBox.Show(error.Exception.Message);
        }

        void AdjustUnit()
        {
            switch ((ConsumableType) type)
            {
                case ConsumableType.Feed:
                    Unit = "kg";
                    IsUnitReadOnly = true;
                    break;

                default:
                    IsUnitReadOnly = false;
                    break;
            }
        }

        void UnsubscribeMessages()
        {
            messageBroker.Unsubscribe(CommonMessages.NewConsumableEntry, OnNewConsumable);
            messageBroker.Unsubscribe(CommonMessages.LoadConsumable, OnLoadConsumable);
            messageBroker.Unsubscribe(CommonMessages.SaveConsumableFailed, OnSaveConsumableFailed);
        }

        public override void Dispose()
        {
            UnsubscribeMessages();
            base.Dispose();
        }
    }
}
