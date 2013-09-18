using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Commands;
using EggFarmSystem.Models;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace EggFarmSystem.Client.Modules.MasterData.ViewModels
{
    public class ConsumableListViewModel : ViewModelBase
    {
        private readonly IMessageBroker messageBroker;
        private readonly IConsumableService consumableService;

        private ObservableCollection<Consumable> consumables;

        public ConsumableListViewModel(IMessageBroker messageBroker, IConsumableService consumableService,
            NewConsumableCommand newCommand, EditConsumableCommand editCommand, DeleteConsumableCommand deleteCommand)
        {
            this.messageBroker = messageBroker;
            this.consumableService = consumableService;

            NewCommand = newCommand;
            EditCommand = editCommand;
            DeleteCommand = deleteCommand;

            NavigationCommands = new List<CommandBase>(){ NewCommand, DeleteCommand };
            SubscribeMessages();
        }

        #region commands

        public NewConsumableCommand NewCommand { get; private set; }

        public EditConsumableCommand EditCommand { get; private set; }

        public DeleteConsumableCommand DeleteCommand { get; private set; }

        public IList<CommandBase> NavigationCommands { get; private set; } 

        #endregion

        #region text

        public string NameText { get { return LanguageData.Consumable_NameField; } }

        public string TypeText { get { return LanguageData.Consumable_TypeField; } }

        public string UnitText { get { return LanguageData.Consumable_UnitField; } }

        public string UnitPriceText { get { return LanguageData.Consumable_UnitPriceField; } }

        public string ActiveText { get { return LanguageData.Consumable_ActiveField; } }

        #endregion

        public ObservableCollection<Consumable> Consumables
        {
            get { return consumables; }
            set 
            { 
                consumables = value;
                OnPropertyChanged("Consumables");
            }
        }

        void SubscribeMessages()
        {
            messageBroker.Subscribe(CommonMessages.RefreshConsumableList, OnConsumableRefresh);
            messageBroker.Subscribe(CommonMessages.DeleteConsumableFailed, OnDeleteConsumableFailed);
        }

        void OnConsumableRefresh(object param)
        {
            var consumableList = consumableService.GetAll();
            Consumables = new ObservableCollection<Consumable>(consumableList);
            DeleteCommand.EntityId = Guid.Empty;
        }

        void OnDeleteConsumableFailed(object param)
        {
            var error = param as Error;
            if (error == null) return;
            MessageBox.Show(error.Exception.Message);
        }

        void UnsubscribeMessages()
        {
            messageBroker.Unsubscribe(CommonMessages.RefreshConsumableList, OnConsumableRefresh);
            messageBroker.Unsubscribe(CommonMessages.DeleteConsumableFailed, OnDeleteConsumableFailed);
        }
        public override void Dispose()
        {
            UnsubscribeMessages();
            base.Dispose();
        }
    }
}
