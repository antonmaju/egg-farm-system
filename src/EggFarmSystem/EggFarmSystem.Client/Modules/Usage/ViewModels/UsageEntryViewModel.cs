using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using EggFarmSystem.Client.Commands;
using EggFarmSystem.Client.Core;
using EggFarmSystem.Client.Modules.Usage.Commands;
using EggFarmSystem.Client.Modules.Usage.Views;
using EggFarmSystem.Models;
using EggFarmSystem.Resources;
using EggFarmSystem.Services;

namespace EggFarmSystem.Client.Modules.Usage.ViewModels
{
    public class UsageEntryViewModel : ViewModelBase
    {
        private readonly IMessageBroker messageBroker;
        private readonly IConsumableUsageService usageService;
        private readonly IConsumableService consumableService;
        private readonly IHenHouseService houseService;

        private DelegateCommand saveCommand, addDetailCommand, deleteDetailCommand;
        private SaveUsageCommand saveUsageCommand;

        public UsageEntryViewModel(IMessageBroker messageBroker, IConsumableUsageService usageService,
            IHenHouseService houseService, IConsumableService consumableService,
            SaveUsageCommand saveUsageCommand, CancelCommand cancelCommand
            )
        {
            this.messageBroker = messageBroker;
            this.usageService = usageService;
            this.houseService = houseService;
            this.consumableService = consumableService;

            this.saveUsageCommand = saveUsageCommand;

            saveCommand = new DelegateCommand(Save, CanSave);
            saveCommand.Text = () => LanguageData.General_Save;

            addDetailCommand = new DelegateCommand(AddDetail, CanAddDetail);
            addDetailCommand.Text = () => LanguageData.General_New;

            CancelCommand = cancelCommand;
            cancelCommand.Action = broker => broker.Publish(CommonMessages.ChangeMainView, typeof(IUsageListView));

            NavigationCommands = new List<CommandBase>(){saveCommand, CancelCommand};

            HouseList = new ObservableCollection<HenHouse>(houseService.GetAll());
            ConsumableList = new ObservableCollection<Consumable>(consumableService.GetAll());

            SubscribeMessages();
        }

        public DelegateCommand AddDetailCommand { get { return addDetailCommand; } }

        public CancelCommand CancelCommand { get; private set; }

        public IList<CommandBase> NavigationCommands { get; private set; }

        public ObservableCollection<HenHouse> HouseList { get; private set; }

        public ObservableCollection<Consumable> ConsumableList { get; private set; } 

        #region text

        public string DateText { get { return LanguageData.Usage_DateField; } }

        public string TotalText { get { return LanguageData.Usage_TotalField; } }

        public string HouseText { get { return LanguageData.Usage_HouseField; } }

        public string ConsumableText { get { return LanguageData.Usage_ConsumableField; } }

        public string CountText { get { return LanguageData.Usage_CountField; } }

        public string UnitPriceText { get { return LanguageData.Usage_UnitPriceField; } }

        public string SubTotalText { get { return LanguageData.Usage_SubTotalField; } }

        #endregion

        #region properties

        private Guid id;
        private DateTime date;
        private long total;
        
        private ObservableCollection<UsageDetailViewModel> details;

        public Guid Id
        {
            get { return id; }
            set 
            { 
                id = value;
                OnPropertyChanged("Id");
            }
        }

        public DateTime Date
        {
            get { return date; }
            set 
            { 
                date = value;
                OnPropertyChanged("Date");
            }
        }

        public long Total
        {
            get { return total; }
            set 
            { 
                total = value;
                OnPropertyChanged("Total");
            }
        }

        public ObservableCollection<UsageDetailViewModel> Details
        {
            get { return details; }
            set 
            { 
                details = value;
                OnPropertyChanged("Details");
            }
        }

        #endregion

        void SubscribeMessages()
        {
            messageBroker.Subscribe(CommonMessages.NewUsageEntry, OnNewUsage);
            messageBroker.Subscribe(CommonMessages.LoadUsage, OnLoadUsage);
            messageBroker.Subscribe(CommonMessages.SaveUsageFailed, OnSaveUsageFailed);
        }

        void OnNewUsage(object param)
        {
            Id = Guid.Empty;
            Total = 0;
            Date = DateTime.Today;
            Details = new ObservableCollection<UsageDetailViewModel>();
            Details.Add(new UsageDetailViewModel());
        }

        void OnLoadUsage(object param)
        {
            var loadedUsage = usageService.Get((Guid) param);

            Id = loadedUsage.Id;
            Total = loadedUsage.Total;
            Date = loadedUsage.Date;
            //Details = new ObservableCollection<Us>(loadedUsage.Details);
            
        }

        void OnSaveUsageFailed(object param)
        {
            
        }

        void UnsubscribeMessages()
        {
            messageBroker.Unsubscribe(CommonMessages.NewUsageEntry, OnNewUsage);
            messageBroker.Unsubscribe(CommonMessages.LoadUsage, OnLoadUsage);
            messageBroker.Unsubscribe(CommonMessages.SaveUsageFailed, OnSaveUsageFailed);
        }

        void Save(object param)
        {
            var v = details;
            Debugger.Break();
        }

        bool CanSave(object param)
        {
            return true;
        }

        void AddDetail(object param)
        {
            details.Add(new UsageDetailViewModel());
        }

        bool CanAddDetail(object param)
        {
            return true;
        }

        public override void Dispose()
        {
            UnsubscribeMessages();
            base.Dispose();
        }
    }
}
