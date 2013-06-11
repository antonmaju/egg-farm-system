using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Commands;
using EggFarmSystem.Models;
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

            NavigationCommands = new List<CommandBase> {NewCommand, EditCommand, DeleteCommand};

            SubscribeMessages();
        }

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
            var houseList = houseService.GetAll();
            Houses = new ObservableCollection<HenHouse>(houseList);
        }

        void OnDeleteHouseFailed(object param)
        {
            MessageBox.Show(param.ToString());
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
