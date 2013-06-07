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

namespace EggFarmSystem.Client.Modules.MasterData.ViewModels
{
    public class HenListViewModel : ViewModelBase
    {
        private readonly IMessageBroker messageBroker;
        private readonly IHenService henService;
        private ObservableCollection<Hen> hens; 

        public HenListViewModel(IMessageBroker messageBroker, 
            IHenService henService, 
            NewHenCommand newHenCommand,
            EditHenCommand editHenCommand)
        {
            this.messageBroker = new MessageBroker();
            NewHenCommand = newHenCommand;
            EditHenCommand = editHenCommand;

            hens = new ObservableCollection<Hen>();
            NavigationCommands = new List<CommandBase>() {NewHenCommand};
            SubscribeMessages();
        }

        void SubscribeMessages()
        {
            messageBroker.Subscribe(CommonMessages.RefreshHenList, OnHenRefresh);
        }

        void OnHenRefresh(object param)
        {
            var henList = henService.GetAll();
            Hens = new ObservableCollection<Hen>(henList);
        }

        void UnsubsribeMessages()
        {
            messageBroker.Unsubscribe(CommonMessages.RefreshHenList, OnHenRefresh);
        }

        public NewHenCommand NewHenCommand { get; private set; }

        public EditHenCommand EditHenCommand { get; private set; } 

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
