using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Commands;
using EggFarmSystem.Models;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.MasterData.ViewModels
{
    public class AdditionalCostListViewModel : ViewModelBase
    {
        private readonly IAdditionalCostService costService;
        private readonly IMessageBroker broker;
        

        public AdditionalCostListViewModel(IMessageBroker broker, IAdditionalCostService costService,
                                           NewAdditionalCostCommand newCommand, EditAdditionalCostCommand editCommand,
                                           DeleteAdditionalCostCommand deleteCommand)
        {
            this.broker = broker;
            this.costService = costService;

            NewCommand = newCommand;
            EditCommand = editCommand;
            DeleteCommand = deleteCommand;
            NavigationCommands =new List<CommandBase>(){NewCommand, DeleteCommand};

            costList = new ObservableCollection<AdditionalCost>();
            SubscribeMessages();
        }

        #region commands

        public NewAdditionalCostCommand NewCommand { get; private set; }

        public EditAdditionalCostCommand EditCommand { get; private set; }

        public DeleteAdditionalCostCommand DeleteCommand { get; private set; }

        public IList<CommandBase> NavigationCommands { get; private set; } 

        #endregion

        #region text

        public string NameText { get { return LanguageData.AdditionalCost_NameField; } }

        public string ValueText { get { return LanguageData.AdditionalCost_ValueField; } }

        #endregion

        private ObservableCollection<AdditionalCost> costList;

        public ObservableCollection<AdditionalCost> CostList
        {
            get { return costList; }
            set { 
                costList = value;
                OnPropertyChanged("CostList");
            }
        }

        void SubscribeMessages()
        {
            broker.Subscribe(CommonMessages.RefreshAdditionalCostList, OnRefreshCostList);
            broker.Subscribe(CommonMessages.DeleteAdditionalCostFailed, OnDeleteCostFailed);
        }

        void OnRefreshCostList(object param)
        {
            var costList = costService.GetAll();
            CostList = new ObservableCollection<AdditionalCost>(costList);
            DeleteCommand.EntityId = Guid.Empty;
        }

        void OnDeleteCostFailed(object param)
        {
            MessageBox.Show(LanguageHelper.TryGetErrorMessage(param));
        }
        
        void UnsubscribeMessages()
        {
            broker.Unsubscribe(CommonMessages.RefreshAdditionalCostList, OnRefreshCostList);
            broker.Unsubscribe(CommonMessages.DeleteAdditionalCostFailed, OnDeleteCostFailed);
        }

        public override void Dispose()
        {
            UnsubscribeMessages();
            base.Dispose();
        }
    }

}
