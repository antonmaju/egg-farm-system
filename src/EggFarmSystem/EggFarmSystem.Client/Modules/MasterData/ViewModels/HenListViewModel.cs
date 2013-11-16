using System.Windows.Input;
using AutoMapper;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.MasterData.Commands;
using EggFarmSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;
using System.Windows;

namespace EggFarmSystem.Client.Modules.MasterData.ViewModels
{
    public class HenListViewModel : ViewModelBase
    {
        private readonly IMessageBroker messageBroker;
        private readonly IHenService henService;
        private readonly IHenHouseService houseService;
        private ObservableCollection<HenListItem> hens; 

        public HenListViewModel(IMessageBroker messageBroker,IHenService henService, IHenHouseService houseService,
            NewHenCommand newHenCommand,EditHenCommand editHenCommand, DeleteHenCommand deleteCommand)
        {
            this.henService = henService;
            this.messageBroker = messageBroker;
            this.houseService = houseService;

            NewCommand = newHenCommand;
            EditCommand = editHenCommand;
            DeleteCommand = deleteCommand;


            hens = new ObservableCollection<HenListItem>();
            NavigationCommands = new List<CommandBase>() {NewCommand, DeleteCommand};
            SubscribeMessages();
        }

        #region text

        public string NameText { get { return LanguageData.Hen_NameField; } }

        public string TypeText { get { return LanguageData.Hen_TypeField; } }

        public string CountText { get { return LanguageData.Hen_CountField; } }

        public string ActiveText { get { return LanguageData.Hen_ActiveField; } }

        public string HouseText { get { return LanguageData.Hen_HouseField; } }

        #endregion

        void SubscribeMessages()
        {
            messageBroker.Subscribe(CommonMessages.RefreshHenList, OnHenRefresh);
            messageBroker.Subscribe(CommonMessages.DeleteHenFailed, OnDeleteFailed);
        }

        void OnHenRefresh(object param)
        {
            IList<Hen> henList = null;
            henList =  henService.GetAll();
            var henListItem = Mapper.Map<IList<Hen>, IList<HenListItem>>(henList);
            var houseList = houseService.GetAll();
            
            foreach (var listItem in henListItem)
            {
                var house = houseList.FirstOrDefault(h => h.Id == listItem.HouseId);
                if (house == null) continue;

                listItem.HouseName = house.Name;
            }

            Hens = new ObservableCollection<HenListItem>(henListItem);
            DeleteCommand.EntityId = Guid.Empty;
        }

        void OnDeleteFailed(object param)
        {
            MessageBox.Show(LanguageHelper.TryGetErrorMessage(param)) ;
        }

        void UnsubsribeMessages()
        {
            messageBroker.Unsubscribe(CommonMessages.DeleteHenFailed, OnDeleteFailed);
            messageBroker.Unsubscribe(CommonMessages.RefreshHenList, OnHenRefresh);
        }

        public NewHenCommand NewCommand { get; private set; }

        public EditHenCommand EditCommand { get; private set; }

        public DeleteHenCommand DeleteCommand { get; private set; }

        public ObservableCollection<HenListItem> Hens
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

    public class HenListItem : Models.Hen
    {
        public string HouseName { get; set; } 
    }
}
