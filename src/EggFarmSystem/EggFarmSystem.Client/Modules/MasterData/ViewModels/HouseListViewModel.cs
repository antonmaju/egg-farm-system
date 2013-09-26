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
    public class HouseListViewModel : ViewModelBase
    {
        private readonly IMessageBroker messageBroker;
        private readonly IHenHouseService houseService;

        private ObservableCollection<HenHouse> houses;

        public HouseListViewModel(IMessageBroker messageBroker, IHenHouseService houseService,
            NewHouseCommand newHouseCommand, EditHouseCommand editHouseCommand, DeleteHouseCommand deleteHouseCommand)
        {
            this.messageBroker = messageBroker;
            this.houseService = houseService;

            NewCommand = newHouseCommand;
            EditCommand = editHouseCommand;
            DeleteCommand = deleteHouseCommand;

            NavigationCommands = new List<CommandBase> {NewCommand, DeleteCommand};

            SubscribeMessages();
        }

        #region text

        public string NameText { get { return LanguageData.House_NameField; } }

        public string PurchaseCostText { get { return LanguageData.House_PurchaseCostField; } }

        public string YearUsageText { get { return LanguageData.House_YearUsageField; } }

        public string ActiveText { get { return LanguageData.House_ActiveField; } }

        #endregion

        public NewHouseCommand NewCommand { get; private set; }

        public EditHouseCommand EditCommand { get; private set; }

        public DeleteHouseCommand DeleteCommand { get; private set; }

        public IList<CommandBase> NavigationCommands { get; private set; }  

        public ObservableCollection<HenHouse> Houses
        {
            get { return houses; }
            set {
                houses = value;
                OnPropertyChanged("Houses");
            }
        }
    
        void SubscribeMessages()
        {
            messageBroker.Subscribe(CommonMessages.RefreshHouseList, OnHouseRefresh);
            messageBroker.Subscribe(CommonMessages.DeleteHouseFailed, OnDeleteHouseFailed);
        }
        
        void OnHouseRefresh(object param)
        {
            var houseList = houseService.GetAll().OrderBy(h =>h.Name).ToList();
            if(houseList == null)
                houseList = new List<HenHouse>();
            Houses = new ObservableCollection<HenHouse>(houseList);
            DeleteCommand.EntityId = Guid.Empty;
        }

        void OnDeleteHouseFailed(object param)
        {
            MessageBox.Show(LanguageHelper.TryGetErrorMessage(param));
        }

        void UnsubscribeMessages()
        {
            messageBroker.Unsubscribe(CommonMessages.RefreshHouseList, OnHouseRefresh);
            messageBroker.Unsubscribe(CommonMessages.DeleteHouseFailed, OnDeleteHouseFailed);
        }

        public override void Dispose()
        {
            UnsubscribeMessages();
            base.Dispose();
        }

    }
}
