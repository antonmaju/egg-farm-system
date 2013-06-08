using System.Windows.Input;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Commands;
using EggFarmSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using EggFarmSystem.Services;
using System.Windows;

namespace EggFarmSystem.Client.Modules.MasterData.ViewModels
{
    public class HenListViewModel : ViewModelBase
    {
        private readonly IMessageBroker messageBroker;
        private readonly IHenService henService;
        private ObservableCollection<Hen> hens; 

        public HenListViewModel(IMessageBroker messageBroker,IHenService henService, 
            NewHenCommand newHenCommand,EditHenCommand editHenCommand, DeleteHenCommand deleteCommand)
        {
            this.henService = henService;
            this.messageBroker = messageBroker;
            NewCommand = newHenCommand;
            EditCommand = editHenCommand;
            DeleteCommand = deleteCommand;

            hens = new ObservableCollection<Hen>();
            NavigationCommands = new List<CommandBase>() {NewCommand, DeleteCommand};
            SubscribeMessages();
        }

        void SubscribeMessages()
        {
            messageBroker.Subscribe(CommonMessages.RefreshHenList, OnHenRefresh);
            messageBroker.Subscribe(CommonMessages.DeleteHenFailed, OnDeleteFailed);
        }

        void OnHenRefresh(object param)
        {
            var henList = henService.GetAll();
            Hens = new ObservableCollection<Hen>(henList);         
        }

        void OnDeleteFailed(object param)
        {
            MessageBox.Show(param.ToString());
        }

        void UnsubsribeMessages()
        {
            messageBroker.Unsubscribe(CommonMessages.DeleteHenFailed, OnDeleteFailed);
            messageBroker.Unsubscribe(CommonMessages.RefreshHenList, OnHenRefresh);
        }

        public NewHenCommand NewCommand { get; private set; }

        public EditHenCommand EditCommand { get; private set; }

        public DeleteHenCommand DeleteCommand { get; private set; }

        public ObservableCollection<Hen> Hens
        {
            get { return hens; }
            private set { 
                hens = value;
                OnPropertyChanged("Hens");
            }
        }

        public IList<CommandBase> NavigationCommands { get; private set; }

        public override void Dispose()
        {
            UnsubsribeMessages();
            base.Dispose();
        }
    }
}
