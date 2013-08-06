using System.Windows;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Commands;
using EggFarmSystem.Models;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggFarmSystem.Client.Modules.MasterData.ViewModels
{
    public class AdditionalCostEntryViewModel : ViewModelBase
    {
        private readonly IMessageBroker broker;
        private readonly IAdditionalCostService costService;

        public AdditionalCostEntryViewModel(IMessageBroker broker, IAdditionalCostService costService,
                                            SaveAdditionalCostCommand saveCommand, CancelCommand cancelCommand)
        {
            this.broker = broker;
            this.costService = costService;

            ActualSaveCommand = saveCommand;
            cancelCommand.Action = mBroker => mBroker.Publish(CommonMessages.ChangeMasterDataView, MasterDataTypes.AdditionalCost);
            SaveCommand = new DelegateCommand(Save, CanSave){Text = ()=> LanguageData.General_Save};
            NavigationCommands = new List<CommandBase>(){SaveCommand, cancelCommand};
            
            SubscribeMessages();
        }

        #region commands

        public IList<CommandBase> NavigationCommands { get; private set; }

        public DelegateCommand SaveCommand { get; private set; }

        private SaveAdditionalCostCommand ActualSaveCommand { get; set; }

        void Save(object param)
        {
            var cost = AutoMapper.Mapper.Map<AdditionalCostEntryViewModel, AdditionalCost>(this);
            ActualSaveCommand.Entity = cost;
            ActualSaveCommand.Execute(null);
        }

        bool CanSave(object param)
        {
            return IsValid();
        }

        #endregion

        #region properties

        private Guid id;
        private string name;
        private long value;

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
            set { 
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public long Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged("Value");
            }
        }

        #endregion

        #region message handlers

        void SubscribeMessages()
        {
            broker.Subscribe(CommonMessages.NewAdditionalCostEntry, OnNew);
            broker.Subscribe(CommonMessages.LoadAdditionalCost, OnLoad);
            broker.Subscribe(CommonMessages.SaveAdditionalCostFailed, OnSaveFailed);
        }

        void UnsubscribeMessages()
        {
            broker.Unsubscribe(CommonMessages.NewAdditionalCostEntry, OnNew);
            broker.Unsubscribe(CommonMessages.LoadAdditionalCost, OnLoad);
            broker.Unsubscribe(CommonMessages.SaveAdditionalCostFailed, OnSaveFailed);
        }

        void OnNew(object param)
        {
            Id = Guid.Empty;
            Name = string.Empty;
            Value = 0;
        }

        void OnLoad(object param)
        {
            var cost = costService.Get((Guid) param);
            Id = cost.Id;
            Name = cost.Name;
            Value = cost.Value;
        }

        void OnSaveFailed(object param)
        {
            MessageBox.Show(LanguageHelper.TryGetErrorMessage(param));
        }

        #endregion

        #region validation

        private static readonly string[] PropertiesToValidate =
            {
                "Id",
                "Name",
                "Value"
            };

        public override string this[string columnName]
        {
            get 
            {             
                string result = null;

                switch (columnName)
                {
                    case "Name":
                        if (string.IsNullOrWhiteSpace(Name))
                            result = LanguageData.AdditionalCost_RequireName;
                        break;

                    case "Value":
                        if (Value < 0)
                            result = LanguageData.AdditionalCost_RequireValue;
                        break;
                }

                return result;
            }
        }

        private bool IsValid()
        {
            bool isValid = true;

            foreach (var prop in PropertiesToValidate)
            {
                if (this[prop] != null)
                {
                    isValid = false;
                    break;
                }
            }

            return isValid;
        }

        #endregion

        #region text

        public string NameText { get { return LanguageData.AdditionalCost_NameField; } }

        public string ValueText { get { return LanguageData.AdditionalCost_ValueField; } }

        #endregion

        public override void Dispose()
        {
            UnsubscribeMessages();
            base.Dispose();
        }
    }
}
